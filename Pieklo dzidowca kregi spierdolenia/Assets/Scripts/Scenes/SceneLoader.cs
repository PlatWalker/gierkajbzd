using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using jbzd.MainHero;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace jbzd.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [field:SerializeField]
        public Vector3 PlayerPositionOnNewMap { get; set; }
        
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

        [field: Dropdown(nameof(AllKregi), nameof(OnValidate))]
        public string chosenKrag;

        [Dropdown(nameof(SceneNames))]
        public string levelNameOfSceneToLoad;
        
        private PlayerManager _playerManager;
        [Inject]
        public void Constructor(PlayerManager playerManager)
        {
            _playerManager = playerManager;
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
                    case SceneType.SingleLoad:
                        SceneNames.Add(iteratedScene.LevelName);
                        break;
                    case SceneType.Passive:
                        
                        var isThereInteractiveScene = scenesData.Any(x =>
                        {
                            var scene = new JbzdScene(x);
                            
                            return x.enabled &&
                                   scene.SceneType == SceneType.Interactive &&
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
                    case SceneType.Interactive:
                        var isTherePassiveScene = scenesData.Any(x =>
                        {
                            var scene = new JbzdScene(x);

                            return x.enabled &&
                                   scene.SceneType == SceneType.Passive &&
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
            Debug.Assert(EditorBuildSettings.scenes.Length != 0, "there are no scenes registered");
            var sceneOfThisTrigger = new JbzdScene(EditorBuildSettings.scenes.First(scene => scene.path == gameObject.scene.path));
            try
            {
                if (chosenKrag == null) throw new Exception("Krag is not set");
                
                if (sceneOfThisTrigger.KragType == chosenKrag)
                {
                    LoadSceneFromSameKrag();
                }
                else if (sceneOfThisTrigger.KragType != chosenKrag)
                {
                    LoadSceneFromDiffrentKrag();
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Reloading scene failed: " + e);
            }
            
            void LoadSceneFromDiffrentKrag()
            {
                var interactiveSceneToLoad = EditorBuildSettings.scenes.First(sceneData =>
                {
                    var tempScene = new JbzdScene(sceneData);
                    return tempScene.LevelName == levelNameOfSceneToLoad &&
                           tempScene.SceneType == SceneType.Interactive;
                });
                var passiveSceneToLoad = EditorBuildSettings.scenes.First(sceneData =>
                {
                    var tempScene = new JbzdScene(sceneData);
                    return tempScene.LevelName == levelNameOfSceneToLoad &&
                           tempScene.SceneType == SceneType.Passive;
                });
                
                SceneManager.LoadSceneAsync(interactiveSceneToLoad.path, LoadSceneMode.Additive);
                SceneManager.LoadSceneAsync(passiveSceneToLoad.path, LoadSceneMode.Additive);
                
                _playerManager.PlaceAt(PlayerPositionOnNewMap);
                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var iteratedScene = SceneManager.GetSceneAt(i);
                    var jbzdIteratedScene = new JbzdScene(EditorBuildSettings.scenes.First(scene => scene.path == iteratedScene.path));
                    
                    if(jbzdIteratedScene.SceneType == SceneType.SingleLoad ||
                       sceneOfThisTrigger.KragType == chosenKrag) continue;
                    
                    SceneManager.UnloadSceneAsync(iteratedScene);
                }
            }
            
            void LoadSceneFromSameKrag()
            {
                var interactiveSceneToLoad = EditorBuildSettings.scenes.First(sceneData =>
                {
                    var tempScene = new JbzdScene(sceneData);
                    return tempScene.LevelName == levelNameOfSceneToLoad &&
                           tempScene.SceneType == SceneType.Interactive;
                });

                if (!SceneManager.GetSceneByPath(interactiveSceneToLoad.path).isLoaded)
                {
                    SceneManager.LoadSceneAsync(interactiveSceneToLoad.path, LoadSceneMode.Additive);
                }

                var passiveSceneToLoad = EditorBuildSettings.scenes.First(sceneData =>
                {
                    var tempScene = new JbzdScene(sceneData);
                    return tempScene.LevelName == levelNameOfSceneToLoad &&
                           tempScene.SceneType == SceneType.Passive;
                });

                SceneManager.LoadSceneAsync(passiveSceneToLoad.path, LoadSceneMode.Additive);

                Debug.Assert(_playerManager is not null, "Zenject didnt injected player manager");
                _playerManager.PlaceAt(PlayerPositionOnNewMap);
                SceneManager.UnloadSceneAsync(gameObject.scene);
            }
        }
    }
}