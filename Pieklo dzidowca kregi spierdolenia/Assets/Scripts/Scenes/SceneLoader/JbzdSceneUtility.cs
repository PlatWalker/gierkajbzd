using System.Collections.Generic;
using System.Linq;
using jbzd.Scenes.SceneLoader.ValueTypes;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jbzd.Scenes.SceneLoader
{
    public static class JbzdSceneUtility
    {
        [CanBeNull]
        public static string GetSceneType(string sceneName)
        {
            if (sceneName.Contains(SceneTypes.SingleLoad)) return SceneTypes.SingleLoad;
            if (sceneName.Contains(SceneTypes.Passive)) return SceneTypes.Passive;
            if (sceneName.Contains(SceneTypes.Interactive)) return SceneTypes.Interactive;
            
            Debug.LogError($"scene {sceneName} should contain '(SingleLoad)' or '(Passive)' or '(Interactive)'");
            return null;
        }
        
        public static string GetLevelName(string sceneName)
        {
            var indexOfSecondOpenBracket = sceneName.IndexOf("(", sceneName.IndexOf("(") + 1);
            var withoutFirstBrackets = sceneName.Substring(indexOfSecondOpenBracket);
            var start = withoutFirstBrackets.IndexOf(")") + 1;
            var length = withoutFirstBrackets.Length;
            return withoutFirstBrackets.Remove(start, length - start);
        }
        
        public static string GetKragType(string sceneName)
        {
            var firstBrackets = sceneName.Substring(0, sceneName.IndexOf(")") + 1);

            var kragTypeName =  firstBrackets switch
            {
                Kregi.Krag1 => Kregi.Krag1,
                Kregi.Krag2 => Kregi.Krag2,
                Kregi.Krag3 => Kregi.Krag3,
                Kregi.Krag4 => Kregi.Krag4,
                Kregi.Krag5 => Kregi.Krag5,
                Kregi.Krag6 => Kregi.Krag6,
                Kregi.Krag7 => Kregi.Krag7,
                Kregi.Krag8 => Kregi.Krag8,
                Kregi.Krag9 => Kregi.Krag9,
                Kregi.None => Kregi.None,
                _ => "error"
            };

            if (kragTypeName != "error") return firstBrackets;
            
            Debug.LogError($"scene {sceneName} has wrongly formatted krag type");
            return null;
        }

        public static string MergeFullSceneName(Krag krag, LevelName levelName, SceneType sceneType)
        {
            return krag + " - " + levelName + " - " + sceneType;
        }
        
        public static List<Scene> GetInteractiveScenes(IEnumerable<Scene> scenesToFilter)
        {
            return scenesToFilter
                .Where(openedScene => GetSceneType(openedScene.name) == SceneTypes.Interactive)
                .ToList();
        }

        public static List<Scene> GetOpenedInteractiveScenes() => GetInteractiveScenes(GetOpenedScenes());

        public static IEnumerable<Scene> GetOpenedScenes()
        {
            var countLoaded = SceneManager.sceneCount;
            var loadedScenes = new Scene[countLoaded];
 
            for (var i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            return loadedScenes;
        }
    }
}