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

        private ExperimentalItemDropBase[] itemToDrop;
        
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

            objectWithDrop = (GameObject)EditorGUILayout.ObjectField("This drop will sit on: ", objectWithDrop, typeof(GameObject), true);
            
            if(objectWithDrop == null)
                EditorGUILayout.HelpBox("Ale wpierdol obiekt gdzie drop ma siedzieć!", MessageType.Warning, true);
            
            howManyItems = EditorGUILayout.IntSlider("Number of items in drop: ", howManyItems, 1, 10);

            if (GUILayout.Button("Generate " + howManyItems + " item drops"))
            {
                objectWithDrop.AddComponent<ExperimentalItemDrop>();
                //objectWithDrop.GetComponent<ExperimentalItemDrop>().itemDrops = new ExperimentalItemDropBase[howManyItems];
                SpawnItemDrops();
            }
            
            GUILayout.EndVertical();
        }

        private void SpawnItemDrops()
        {
            /*ExperimentalItemDropBase[] itemDrops = objectWithDrop.GetComponent<ExperimentalItemDrop>().itemDrops;
            
            foreach (ExperimentalItemDropBase drop in itemDrops)
            {
                drop.dropItem = (GameObject)EditorGUILayout.ObjectField("This item will drop: ", drop.dropItem, typeof(GameObject), true);
                drop.dropChance = EditorGUILayout.FloatField("With this chance: ", drop.dropChance);
            }*/
        }
    }

    public class ItemDropCustomEditor : Editor
    {
        private ExperimentalItemDropBase[] newDropBase;

        public override void OnInspectorGUI()
        {
            DrawDropItem();
        }

        private void DrawDropItem()
        {
            Debug.Log("hehe");
        }
    }
}

