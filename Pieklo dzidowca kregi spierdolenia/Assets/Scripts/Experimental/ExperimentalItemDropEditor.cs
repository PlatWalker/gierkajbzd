using System;
using System.CodeDom.Compiler;
using UnityEditor;
using UnityEngine;
using jbzdy.Items.Drop;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace jbzdy.Items.DropEditor
{
    public class ExperimentalItemDropEditor : EditorWindow
    {
        private GameObject newItemDrop;
        private GameObject objectWithDrop;
        private int howManyItems;
        private string dropName;
        private ItemDrop newDrop;
        
        [MenuItem("Sharashino Tools/DONT CLICK ME")]
        static void Init()
        {
            ExperimentalItemDropEditor _editor = (ExperimentalItemDropEditor) GetWindow(typeof(ExperimentalItemDropEditor));
            _editor.titleContent = new GUIContent("Item Drop Wizard");
            _editor.Show();
        }

        private void OnGUI()
        {
            GUILayout.TextArea("New Item Drop", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");

            dropName = EditorGUILayout.TextField("Name this drop: ", dropName);
            objectWithDrop = (GameObject)EditorGUILayout.ObjectField("This drop will sit on: ", objectWithDrop, typeof(GameObject), true);

            if (objectWithDrop == null)
            {
                EditorGUILayout.HelpBox("Ale wpierdol obiekt gdzie drop ma siedzieć!", MessageType.Warning, true);
            }
            else if (objectWithDrop != null && objectWithDrop.GetComponent<test>() == null)
            {
                EditorGUILayout.HelpBox("Na tym obiekcie nie ma klasy która przechowuje dropy!", MessageType.Warning, true);
            }
            else
            {
                howManyItems = EditorGUILayout.IntSlider("Number of items in drop: ", howManyItems, 1, 10);

                if (GUILayout.Button("Generate " + howManyItems + " item drops"))
                {
                        objectWithDrop.GetComponent<test>().itemDrop = ScriptableObject.CreateInstance<ItemDrop>();
                }
            
                newDrop = objectWithDrop.GetComponent<test>().itemDrop;
                newDrop.itemDropBases = new ItemDropBase[howManyItems];

                for (int i = 0; i < howManyItems; i++)
                {
                    GUILayout.BeginVertical("HelpBox");
                    newDrop.itemDropBases[i] = new ItemDropBase();
                    newDrop.itemDropBases[i].ItemToDrop = (GameObject)EditorGUILayout.ObjectField("This item will drop: ", newDrop.itemDropBases[i].ItemToDrop, typeof(GameObject), true);
                    newDrop.itemDropBases[i].ItemDropChance = EditorGUILayout.FloatField("With this chance: ", newDrop.itemDropBases[i].ItemDropChance);
                    GUILayout.EndVertical();
                }

                
            }
            
            if (GUILayout.Button("Save changes?"))
            {
                EditorUtility.SetDirty(newDrop);
                EditorSceneManager.MarkSceneDirty(objectWithDrop.gameObject.scene);
                var path = AssetDatabase.GUIDToAssetPath(dropName);
                AssetDatabase.RenameAsset(path, dropName);
                
                Debug.Log( AssetDatabase.RenameAsset(path, dropName));
            }
            
            GUILayout.EndVertical();
        }
    }
}

