using JetBrains.Annotations;
using UnityEditor;

namespace jbzd.Scenes.SceneLoader
{
    public class JbzdScene
    {
#if UNITY_EDITOR
        public EditorBuildSettingsScene EditorBuildSettingsScene { get; }

        public string FullSceneName => GetSceneNameFromPath();
        public string KragType => JbzdSceneUtility.GetKragType(FullSceneName);
        public string LevelName => JbzdSceneUtility.GetLevelName(FullSceneName);
        [CanBeNull] public string SceneType => JbzdSceneUtility.GetSceneType(FullSceneName);

        public JbzdScene(EditorBuildSettingsScene editorBuildSettingsScene)
        {
            EditorBuildSettingsScene = editorBuildSettingsScene;
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
#endif
    }
}