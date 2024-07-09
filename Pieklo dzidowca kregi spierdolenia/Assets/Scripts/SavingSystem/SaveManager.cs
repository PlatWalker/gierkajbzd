using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using jbzd.Common.Extensions;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Dialogues.RuntimeData;
using jbzd.SavingSystem.SaveData;
using jbzd.Scenes.SceneLoader;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd.SavingSystem
{
    [UsedImplicitly] // in installer
    public class SaveManager : MonoBehaviour
    {
        private const string SAVE_DIRECTORY_NAME = "SaveData";
        private const string SAVE_FILE_NAME = "SaveData.json";

        private InputManager _inputManager;

        /// <summary>
        /// Dictionary tracks all the gameObjects on interactive scene. So after saving game we will know
        /// if object is destroyed or deactivated. After loading game we can change status of object according to how
        /// it was saved. Key of dictionary is hierarchy of parent game objects of saved game objects. Value is tuple
        /// where first item is name of game object and second item is reference to this game object and third item is
        /// name of scene where game object was
        /// </summary>
        private readonly Dictionary<List<string>, (string, GameObject, string)> _trackGameObjects = new();

        [Inject]
        public void Constructor(InputManager inputManager)
        {
            _inputManager = inputManager;
            var userInterfaceInput = _inputManager.GetInput<UserInterfaceInput>();
            userInterfaceInput.OnQuickSaveClick += SaveGame;
            userInterfaceInput.OnTestLoadClick += LoadGame;
        }

        public void Start()
        {
            TrackReferencesOnInteractiveScenes();
            
            return;
            
            void TrackReferencesOnInteractiveScenes()
            {
                SceneManager.sceneLoaded += TrackReferencesOnNewScene;
                
                var rootGameObjectsFromInteractiveScenes = new List<GameObject>();
                
                foreach (var openedScene in JbzdSceneUtility.GetOpenedInteractiveScenes())
                {
                    rootGameObjectsFromInteractiveScenes.AddRange(openedScene.GetRootGameObjects());
                }
                
                Track(rootGameObjectsFromInteractiveScenes);

                return;
                
                void TrackReferencesOnNewScene(Scene arg0, LoadSceneMode loadSceneMode)
                {
                    if (JbzdSceneUtility.GetSceneType(arg0.name) == SceneTypes.Interactive)
                    {
                        Track(arg0.GetRootGameObjects().ToList());
                    }
                }
                
                void Track(List<GameObject> rootGameObjects)
                {
                    foreach (var rootGameObject in rootGameObjects)
                    {
                        if (!_trackGameObjects.TryAdd(rootGameObject.GetAllParentsNames(), (rootGameObject.name, rootGameObject, rootGameObject.scene.name)))
                        {
                            Debug.LogError("Save manager trying to track gameobject that is already tracked.");
                        }
                    
                        IterateOverChild(rootGameObject);
                    }

                    return;

                    void IterateOverChild(GameObject rootGameObject)
                    {
                        for (var i = 0; i < rootGameObject.transform.childCount; i++)
                        {
                            var child = rootGameObject.transform.GetChild(i);

                            if (child.CompareTag("BlenderModel")) continue;

                            var childGameObject = child.gameObject;
                            _trackGameObjects.TryAdd(childGameObject.GetAllParentsNames(), (child.name, childGameObject, childGameObject.scene.name));
                        
                            IterateOverChild(child.gameObject);
                        }
                    }
                }
            }
        }

        public void SaveGame()
        {
            try
            {
                var gameData = new GameData();

                PopulateByInterface(ref gameData);
                PopulateWithOpenedScenes(ref gameData);
                PopulateWithStatusesOfGameObjects(ref gameData);

                SaveDataToFile(gameData);
                Debug.Log("GAME SAVED");
            }
            catch(Exception e)
            {
                Debug.LogError("Something went wrong during saving game:" + e);
            }

            return;
            
            void PopulateByInterface(ref GameData gameData)
            {
                var saveables = FindSaveables();

                foreach (var saveable in saveables)
                {
                    saveable.SaveData(ref gameData);
                }
            }

            void PopulateWithOpenedScenes(ref GameData gameData)
            {
                var openedScenes = JbzdSceneUtility.GetOpenedScenesExceptSingleLoad();
                gameData.openedScenes.AddRange(openedScenes.Select(scene => scene.name));
            }

            void PopulateWithStatusesOfGameObjects(ref GameData gameData)
            {
                var enumerator = _trackGameObjects.GetEnumerator();
                
                while (enumerator.MoveNext())
                {
                    var (originalGameObjectObjectPath, (originalGameObjectName, originalGameObject, originalGameObjectScene)) = enumerator.Current;

                    if (originalGameObject == null)
                    {
                        gameData.gameObjectsStatusSaveDatas.Add(new DisabledAndDestroyedGameObjectsSaveData
                        (
                            originalGameObjectScene,
                            originalGameObjectObjectPath,
                            originalGameObjectName,
                            DisabledOrDestroyed.Destroyed
                        ));
                    }
                    else if (originalGameObject.activeSelf is false)
                    {
                        gameData.gameObjectsStatusSaveDatas.Add(new DisabledAndDestroyedGameObjectsSaveData(
                            originalGameObjectScene,
                            originalGameObjectObjectPath,
                            originalGameObjectName,
                            DisabledOrDestroyed.Disabled
                        ));
                    }

                }
                
                enumerator.Dispose();
            }
            
            void SaveDataToFile(GameData gameData)
            {
                var directoryPath = Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY_NAME);
                var filePath = Path.Combine(directoryPath, SAVE_FILE_NAME);
            
                try
                {
                    Directory.CreateDirectory(directoryPath);
                
                    var dataToStore = JsonUtility.ToJson(gameData, true);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    using var writer = new StreamWriter(stream);
                
                    writer.Write(dataToStore);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to save data to file: " + directoryPath + "\n" + e);
                }
            }
        }

        public async void LoadGame()
        {
            try
            {
                //TODO show loading page
                var gameData = LoadDataToVariable();

                await LoadOpenedScenes(gameData);
                LoadByInterface(gameData);
                LoadStatusesOfGameObjects(gameData);
                //TODO quit loading page
                
                Debug.Log("GAME LOADED");
            }
            catch (Exception e)
            {
                Debug.LogError("Something went wrong during loading game:" + e);
            }

            return;

            void LoadByInterface(GameData gameData)
            {
                foreach (var saveable in FindSaveables())
                {
                    saveable.LoadData(gameData);
                }
            }

            async Task LoadOpenedScenes(GameData gameData)
            {
                var scenes = JbzdSceneUtility.GetOpenedScenesExceptSingleLoad();
                
                foreach (var scene in scenes)
                {
                    await SceneManager.UnloadSceneAsync(scene);
                }
                
                foreach (var scene in gameData.openedScenes)
                {
                    await SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                }
            }

            void LoadStatusesOfGameObjects(GameData gameData)
            {
                var interactiveScenes = JbzdSceneUtility.GetInteractiveScenes(JbzdSceneUtility.GetOpenedScenes());

                foreach (var interactiveScene in interactiveScenes)
                {
                    var roots = interactiveScene.GetRootGameObjects();
                    
                    var gameObjectsWithStatusToChange = new List<GameObject>();
                
                    var allSaveData = gameData.gameObjectsStatusSaveDatas
                        .Select(item => item.Clone())
                        .Cast<DisabledAndDestroyedGameObjectsSaveData>()
                        .ToList();
                    
                    foreach (var saveData in allSaveData.Where(saveData => saveData.SceneName == interactiveScene.name))
                    {
                        var path = saveData.ObjectPath;

                        var firstParent = roots.First(root => root.name == path.First());
                        path.RemoveAt(0);
                    
                        IterateThroughChildren(path, firstParent.transform, saveData.Name);
                    }

                    foreach (var gameObjectWithWrongStatus in gameObjectsWithStatusToChange)
                    {
                        var saveDataForObject = allSaveData.FirstOrDefault(data => data.Name == gameObjectWithWrongStatus.name);

                        if (saveDataForObject is null)
                        {
                            throw new Exception("Didn't found saved object in json");
                        }
                    
                        switch (saveDataForObject.DisabledOrDestroyed)
                        {
                            case DisabledOrDestroyed.Disabled:
                                gameObjectWithWrongStatus.SetActive(false);
                                break;
                            case DisabledOrDestroyed.Destroyed:
                                Destroy(gameObjectWithWrongStatus);
                                break;
                            case DisabledOrDestroyed.Unrecognized:
                                throw new Exception("During save'ing data status of game object is not set");
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    
                    void IterateThroughChildren(IList<string> path, Transform firstParent, string nameOfObject)
                    {
                        while (true)
                        {
                            if (path.Count is 0)
                            {
                                gameObjectsWithStatusToChange.Add(firstParent.Find(nameOfObject).gameObject);
                                return;
                            }
                            
                            var nextChild = firstParent.Find(path.First());
                            path.RemoveAt(0);

                            firstParent = nextChild;
                        }
                    }
                }
            }
            
            GameData LoadDataToVariable()
            {
                var directoryPath = Path.Combine(Application.persistentDataPath, SAVE_DIRECTORY_NAME);
                var filePath = Path.Combine(directoryPath, SAVE_FILE_NAME);
            
                try
                {
                    var dataToLoad = "";
                    using var stream = new FileStream(filePath, FileMode.Open);
                    using var reader = new StreamReader(stream);
                
                    dataToLoad = reader.ReadToEnd();
                
                    return JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to save data to file: " + directoryPath + "\n" + e);
                    return null;
                }
            }
        }

        private IEnumerable<ISaveable> FindSaveables()
        {
           var saveables1 = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();

           var saveables2 = Resources.FindObjectsOfTypeAll<ContainerSO>().Cast<ISaveable>();
           
           return saveables2.Concat(saveables1);
        }
    }
}