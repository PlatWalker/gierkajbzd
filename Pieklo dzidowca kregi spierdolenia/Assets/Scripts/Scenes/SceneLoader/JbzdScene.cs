using System.Linq;
using jbzd.Scenes.SceneLoader.ValueTypes;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace jbzd.Scenes.SceneLoader
{
    public readonly struct JbzdScene
    {

        public string ScenePath { get; }
        public string FullSceneName => GetSceneNameFromPath(ScenePath);
        public Krag KragType => new(JbzdSceneUtility.GetKragType(FullSceneName));
        public LevelName LevelName => new(JbzdSceneUtility.GetLevelName(FullSceneName));
        public SceneType SceneType => new(JbzdSceneUtility.GetSceneType(FullSceneName));

#if UNITY_EDITOR
        public JbzdScene(EditorBuildSettingsScene editorBuildSettingsScene)
        {
            ScenePath = editorBuildSettingsScene.path;
        }
#endif
        public JbzdScene(Scene unityScene)
        {
            ScenePath = unityScene.path;
        }

        private string GetSceneNameFromPath(string scenePath)
        {
            var slash = scenePath.LastIndexOf('/');
            var nameWithExtension = scenePath.Substring(slash + 1);
            var dot = nameWithExtension.LastIndexOf('.');
            scenePath = nameWithExtension.Substring(0, dot);
            return scenePath;
        }
        
        public JbzdScene(string sceneName)
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;

            for (var i = 0; i < sceneCount; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var sceneNameInPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                if (sceneNameInPath != sceneName) continue;
                
                ScenePath = scenePath;
                return;
            }

            ScenePath = string.Empty;
        }

        public JbzdScene GetPassiveScene()
        {
            if (SceneType == SceneTypes.Passive) return this;
            
            return new JbzdScene(JbzdSceneUtility.MergeFullSceneName(KragType, LevelName, new SceneType(SceneTypes.Passive)));
        }

        public JbzdScene GetInteractiveScene()
        {
            if (SceneType == SceneTypes.Interactive) return this;

            return new JbzdScene(JbzdSceneUtility.MergeFullSceneName(KragType, LevelName, new SceneType(SceneTypes.Interactive)));
        }
    }
}