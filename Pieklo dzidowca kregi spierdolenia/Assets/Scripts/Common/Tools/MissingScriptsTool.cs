using UnityEditor;
using UnityEngine;

namespace jbzd.Common.Tools
{
    public class MissingScriptsTool
    {
        [MenuItem("Tools/Missing Scripts/Find")]
        private static void FindMissingScriptsMenuItem()
        {
            foreach (var gameObject in Object.FindObjectsOfType<GameObject>(true))
            {
                foreach (var component in gameObject.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        Debug.Log($"GameObject found with missing script {gameObject.name}", gameObject);
                        return;
                    }
                }
            }
        }
        
        [MenuItem("Tools/Missing Scripts/Delete")]
        private static void DeleteMissingScriptsMenuItem()
        {
            foreach (var gameObject in Object.FindObjectsOfType<GameObject>(true))
            {
                foreach (var component in gameObject.GetComponentsInChildren<Component>())
                {
                    if (component == null)
                    {
                        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                        Debug.Log($"Missing scripts from {gameObject.name} was deleted", gameObject);
                        return;
                    }
                }
            }
        }
    }
}