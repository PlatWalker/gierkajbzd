using System.IO;
using UnityEditor;
using UnityEngine;

namespace jbzdy.SoUtility
{
    public static class ScriptableObjectUtiity
    {
        public static T CreateAsset<T> (string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T> ();
 
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (path == "") 
            {
                path = "Assets";
            } 
            else if (Path.GetExtension (path) != "") 
            {
                path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
            }
 
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + name + ".asset");
 
            AssetDatabase.CreateAsset (asset, assetPathAndName);
 
            AssetDatabase.SaveAssets ();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow ();
            Selection.activeObject = asset;

            return asset;
        }
    }
}
