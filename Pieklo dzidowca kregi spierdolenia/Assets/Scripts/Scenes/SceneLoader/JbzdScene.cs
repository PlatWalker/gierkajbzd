using jbzd.Scenes.SceneLoader.ValueTypes;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace jbzd.Scenes.SceneLoader
{
    public readonly struct JbzdScene
    {
#if UNITY_EDITOR
        public string ScenePath { get; }
        public string FullSceneName => GetSceneNameFromPath(ScenePath);
        public Krag KragType => new(JbzdSceneUtility.GetKragType(FullSceneName));
        public LevelName LevelName => new(JbzdSceneUtility.GetLevelName(FullSceneName));
        public SceneType SceneType => new(JbzdSceneUtility.GetSceneType(FullSceneName));

        public JbzdScene(EditorBuildSettingsScene editorBuildSettingsScene)
        {
            ScenePath = editorBuildSettingsScene.path;
        }

        public JbzdScene(Scene unityScene)
        {
            ScenePath = unityScene.path;
        }

        public JbzdScene(string sceneName)
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                var sceneNameInPath = System.IO.Path.GetFileNameWithoutExtension(scene.path);

                if (sceneNameInPath != sceneName) continue;
                
                ScenePath = scene.path;
                return;
            }

            ScenePath = string.Empty;
        }
        
        private string GetSceneNameFromPath(string scenePath)
        {
            var slash = scenePath.LastIndexOf('/');
            var nameWithExtension = scenePath.Substring(slash + 1);
            var dot = nameWithExtension.LastIndexOf('.');
            scenePath = nameWithExtension.Substring(0, dot);
            return scenePath;
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
#endif
    }
}