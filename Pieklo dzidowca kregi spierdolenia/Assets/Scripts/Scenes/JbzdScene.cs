using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace jbzd.Scenes
{
    public class JbzdScene
    {
        public EditorBuildSettingsScene EditorBuildSettingsScene { get; }

        public string FullSceneName => GetSceneNameFromPath();
        public string KragType => GetKragType(FullSceneName);
        public string LevelName => GetLevelName(FullSceneName);
        [CanBeNull] public string SceneType => GetSceneType(FullSceneName);

        public JbzdScene(EditorBuildSettingsScene editorBuildSettingsScene)
        {
            EditorBuildSettingsScene = editorBuildSettingsScene;
        }
        
        [CanBeNull]
        private string GetSceneType(string sceneName)
        {
            if (sceneName.Contains(Scenes.SceneType.SingleLoad)) return Scenes.SceneType.SingleLoad;
            if (sceneName.Contains(Scenes.SceneType.Passive)) return Scenes.SceneType.Passive;
            if (sceneName.Contains(Scenes.SceneType.Interactive)) return Scenes.SceneType.Interactive;
            
            Debug.LogError($"scene {sceneName} should contain '(SingleLoad)' or '(Passive)' or '(Interactive)'");
            return null;
        }
        
        private string GetLevelName(string sceneName)
        {
            var indexOfSecondOpenBracket = sceneName.IndexOf("(", sceneName.IndexOf("(") + 1);
            var withoutFirstBrackets = sceneName.Substring(indexOfSecondOpenBracket);
            var start = withoutFirstBrackets.IndexOf(")") + 1;
            var length = withoutFirstBrackets.Length;
            return withoutFirstBrackets.Remove(start, length - start);
        }

        private string GetKragType(string sceneName)
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
        
        private string GetSceneNameFromPath()
        {
            var scenePath = EditorBuildSettingsScene.path;
            var slash = scenePath.LastIndexOf('/');
            var nameWithExtension = scenePath.Substring(slash + 1);
            var dot = nameWithExtension.LastIndexOf('.');
            scenePath = nameWithExtension.Substring(0, dot);
            return scenePath;
        }
    }
}