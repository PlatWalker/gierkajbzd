using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.MainHero;
using jbzd.Plugins.DropdownAttributes.Core.Scripts;
using jbzd.Scenes.SceneLoader.ValueTypes;
using jbzd.UI;
using jbzd.UI.LoadingScene;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd.Scenes.SceneLoader
{
    public class SceneLoader : MonoBehaviour, IDisposable
    {
        public List<string> AllKregi { get; set; } = new()
        {
            Kregi.Krag1,
            Kregi.Krag2,
            Kregi.Krag3,
            Kregi.Krag4,
            Kregi.Krag5,
            Kregi.Krag6,
            Kregi.Krag7,
            Kregi.Krag8,
            Kregi.Krag9
        };
        public List<string> SceneNames { get; set; } = new();
        
        [Tooltip("Check this box if you want to trigger scene loader with collider")]
        [SerializeField]
        private bool colliderTriggerMode = true;
        
        [Tooltip("Check this box if you want to teleport from different scene than this object belongs to")]
        [SerializeField]
        private bool teleportFromDifferentScene;
        [ConditionalField(nameof(teleportFromDifferentScene))]
        [Scene]
        [SerializeField]
        private string sceneFromWhichToTeleport;
        
#if UNITY_EDITOR
        [field: Dropdown(nameof(AllKregi), nameof(OnValidate))]
#endif
        public string chosenKrag;

        [Dropdown(nameof(SceneNames))]
        public string levelNameOfSceneToLoad;

        public int Id;

        private PlayerManager _playerManager;
        private LoadingUI _loadingUI;

        [Inject]
        public void Constructor(PlayerManager playerManager, UserInterfaceManager userInterfaceManager)
        {
            _playerManager = playerManager;
            _loadingUI = userInterfaceManager.GetUIController<LoadingUI>();
        }

        private void Awake()
        {
            Debug.Assert(_playerManager is not null, "Zenject didnt injected player manager");
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            SceneNames.Clear();
            
            var scenesData = EditorBuildSettings.scenes;
            
            foreach (var sceneData in scenesData)
            {
                if (!sceneData.enabled) continue;

                var iteratedScene = new JbzdScene(sceneData);

                if (iteratedScene.KragType != chosenKrag) continue;

                switch (iteratedScene.SceneType)
                {
                    case SceneTypes.SingleLoad:
                        SceneNames.Add(iteratedScene.LevelName);
                        break;
                    case SceneTypes.Passive:
                        
                        var isThereInteractiveScene = scenesData.Any(x =>
                        {
                            var scene = new JbzdScene(x);
                            
                            return x.enabled &&
                                   scene.SceneType == SceneTypes.Interactive &&
                                   scene.LevelName == iteratedScene.LevelName;
                        });
                        
                        if (isThereInteractiveScene)
                        {
                            SceneNames.Add(iteratedScene.LevelName);
                        }
                        else
                        {
                            Debug.LogError("Every passive scene should have Interactive scene! " +
                                           "You cant load passive scene without interactive one");
                        }
                        break;
                    case SceneTypes.Interactive:
                        var isTherePassiveScene = scenesData.Any(x =>
                        {
                            var scene = new JbzdScene(x);

                            return x.enabled &&
                                   scene.SceneType == SceneTypes.Passive &&
                                   scene.LevelName == iteratedScene.LevelName;
                        });
                        
                        Debug.Assert(isTherePassiveScene, "Every interactive scene should have passive scene!");
                        break;
                }
            }
        }
#endif
        
        public void OnTriggerEnter(Collider other)
        {
            if (colliderTriggerMode) LoadScene();
        }

        public void LoadScene()
        {
            if (_loadingUI.isLoading) return;
            
            _loadingUI.ShowLoadingScreen();
            SceneManager.sceneLoaded += OnSceneLoaded;

            var kragTypeOfThisTrigger = JbzdSceneUtility.GetKragType(gameObject.scene.name);
            Debug.Log($"Scene Loader from {gameObject.scene.name} named {gameObject.name} will teleport to {levelNameOfSceneToLoad}");
            try
            {
                if (chosenKrag == null) throw new Exception("Krag is not set");
                
                var desiredInteractiveSceneName = JbzdSceneUtility.MergeFullSceneName(
                    new Krag(chosenKrag),
                    new LevelName(levelNameOfSceneToLoad),
                    new SceneType(SceneTypes.Interactive));
                
                var desiredPassiveSceneName = JbzdSceneUtility.MergeFullSceneName(
                    new Krag(chosenKrag),
                    new LevelName(levelNameOfSceneToLoad),
                    new SceneType(SceneTypes.Passive));

                if (kragTypeOfThisTrigger == chosenKrag)
                {
                    LoadSceneFromSameKrag(desiredInteractiveSceneName, desiredPassiveSceneName);
                }
                else if (kragTypeOfThisTrigger != chosenKrag)
                {
                    LoadSceneFromDifferentKrag(desiredInteractiveSceneName, desiredPassiveSceneName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Reloading scene failed: " + e);
            }

            return;

            void LoadSceneFromDifferentKrag(string interactiveSceneNameToLoad, string passiveSceneNameToLoad)
            {
                SceneManager.LoadSceneAsync(interactiveSceneNameToLoad, LoadSceneMode.Additive);
                SceneManager.LoadSceneAsync(passiveSceneNameToLoad, LoadSceneMode.Additive);
            }
            
            void LoadSceneFromSameKrag(string interactiveSceneNameToLoad, string passiveSceneNameToLoad)
            {
                if (!SceneManager.GetSceneByName(interactiveSceneNameToLoad).isLoaded)
                {
                    SceneManager.LoadSceneAsync(interactiveSceneNameToLoad, LoadSceneMode.Additive);
                }

                SceneManager.LoadSceneAsync(passiveSceneNameToLoad, LoadSceneMode.Additive);               
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var jbzdScene = new JbzdScene(scene);
            
            if (jbzdScene.SceneType != SceneTypes.Passive)
            {
                return;
            }

            var desiredInteractiveScene = SceneManager.GetSceneByName(jbzdScene.GetInteractiveScene().FullSceneName);
            
            if (desiredInteractiveScene.IsValid())
            {
                WarpPlayerToLocationOnNewMap(desiredInteractiveScene);
            }
            else
            {
                Debug.LogError($"Niepoprawna nazwa sceny: {desiredInteractiveScene.name}, teleportacja nie dziala");
            }

            var thisGameObjectScene = new JbzdScene(gameObject.scene.name);
            
            Debug.Assert(thisGameObjectScene.SceneType == SceneTypes.Interactive, "Scene loader powinien byc na scenie interaktywnej!");
            
            var sceneToUnload = SceneManager.GetSceneByName(teleportFromDifferentScene ?
                sceneFromWhichToTeleport :
                thisGameObjectScene.GetPassiveScene().FullSceneName);

            if (sceneToUnload.IsValid())
            {
                var operation = SceneManager.UnloadSceneAsync(sceneToUnload);
                operation.completed += _loadingUI.HideLoadingScreen;
            }
            else
            {
                Debug.LogError($"There is no scene with name: {sceneToUnload.name}, scene object in unity is invalid");
            }
            
            UnsubscribeSceneLoaded();
        }

        private void WarpPlayerToLocationOnNewMap(Scene scene)
        {
            var gameObjects = scene.GetRootGameObjects();
            
            if (gameObjects.Length is 0)
            {
                Debug.LogError("Something went wrong when getting game objects from loaded scene");
                return;
            }

            var gameObjectToTeleportTo = gameObjects.FirstOrDefault(gObject => {

                if (!gObject.CompareTag("TeleportDestination")) return false;

                if (gObject.TryGetComponent<TeleportDestination>(out var destination)) return destination.Id == Id;
                
                Debug.LogError("One of teleport destinations doesnt have script");
                return true;

                }
            );

            if (gameObjectToTeleportTo is null)
            {
                Debug.LogError(gameObjects.FirstOrDefault(gObject => gObject.CompareTag("TeleportDestination")) is not null
                    ? "There is no object with corresponding destination id"
                    : "There is no object with 'TeleportDestination' tag so there is nothing to teleport to");

                return;
            }

            _playerManager.PlaceAt(gameObjectToTeleportTo.transform.position);
            _playerManager.WarpFollowersToPlayer();
        }

        private void UnsubscribeSceneLoaded()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        public void OnDestroy()
        {
            UnsubscribeSceneLoaded();
        }

        public void Dispose()
        {
            UnsubscribeSceneLoaded();
        }
    }
}