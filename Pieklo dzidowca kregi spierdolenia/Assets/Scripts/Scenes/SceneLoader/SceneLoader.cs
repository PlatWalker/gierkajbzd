using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.MainHero;
using jbzd.Plugins.DropdownAttributes.Core.Scripts;
using jbzd.Scenes.SceneLoader.ValueTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd.Scenes.SceneLoader
{
    public class SceneLoader : MonoBehaviour
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
#if UNITY_EDITOR
        [field: Dropdown(nameof(AllKregi), nameof(OnValidate))]
#endif
        public string chosenKrag;

        [Dropdown(nameof(SceneNames))]
        public string levelNameOfSceneToLoad;
        
        private PlayerManager _playerManager;
        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public void Awake()
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
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            var kragTypeOfThisTrigger = JbzdScene.GetKragType(gameObject.scene.name);

            try
            {
                if (chosenKrag == null) throw new Exception("Krag is not set");
                
                var desiredInteractiveSceneName = JbzdScene.MergeFullSceneName(
                    new Krag(chosenKrag),
                    new LevelName(levelNameOfSceneToLoad),
                    new SceneType(SceneTypes.Interactive));
                
                var desiredPassiveSceneName = JbzdScene.MergeFullSceneName(
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
            
            void LoadSceneFromDifferentKrag(string interactiveSceneNameToLoad, string passiveSceneNameToLoad)
            {
                SceneManager.LoadSceneAsync(interactiveSceneNameToLoad, LoadSceneMode.Additive);
                SceneManager.LoadSceneAsync(passiveSceneNameToLoad, LoadSceneMode.Additive);

                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var iteratedScene = SceneManager.GetSceneAt(i);
                    var iteratedSceneType = JbzdScene.GetSceneType(iteratedScene.name);
                    
                    if(iteratedSceneType == SceneTypes.SingleLoad ||
                       kragTypeOfThisTrigger == chosenKrag ||
                       gameObject.scene.name == iteratedScene.name) continue;
                    
                    SceneManager.UnloadSceneAsync(iteratedScene);
                }
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
            if (JbzdScene.GetSceneType(scene.name) != SceneTypes.Passive) return;

            WarpPlayerToLocationOnNewMap(scene);
            
            SceneManager.UnloadSceneAsync(gameObject.scene);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void WarpPlayerToLocationOnNewMap(Scene scene)
        {
            var gameObjects = scene.GetRootGameObjects();
            
            if (gameObjects.Length is 0)
            {
                Debug.LogError("Something went wrong when getting game objects from loaded scene");
                return;
            }

            var gameObjectToTeleportTo = gameObjects.FirstOrDefault(gObject => gObject.CompareTag("TeleportDestination"));

            if (gameObjectToTeleportTo is null)
            {
                Debug.LogError("There is no object with 'TeleportDestination' tag so there is nothing to teleport to");
                return;
            }

            _playerManager.PlaceAt(gameObjectToTeleportTo.transform.position);
            _playerManager.WarpFollowersToPlayer();
        }
        
    }
}