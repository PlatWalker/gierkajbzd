using UnityEditor;
using UnityEngine;
using jbzdy.Items.Drop;
using jbzdy.SoUtility;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace jbzdy.Items.DropEditor
{
    [CustomEditor(typeof(ItemDropEditor))]
    [CanEditMultipleObjects]
    public class ItemDropEditor : EditorWindow
    {
        private GameObject newItemDrop;
        private GameObject objectWithDrop;
        private int howManyItems;
        private string dropName;
        private ItemDrop newDrop;
        
        [MenuItem("Sharashino Tools/New Item Drop")]
        static void Init()
        {
            ItemDropEditor _editor = (ItemDropEditor) GetWindow(typeof(ItemDropEditor));
            _editor.titleContent = new GUIContent("Item Drop Wizard");
            _editor.Show();
        }

        public void OnGUI()
        {
            GUILayout.TextArea("New Item Drop", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");

            dropName = EditorGUILayout.TextField("Name this drop: ", dropName);
            objectWithDrop = (GameObject)EditorGUILayout.ObjectField("This drop will sit on: ", objectWithDrop, typeof(GameObject), true);

            if (objectWithDrop == null)
            {
                EditorGUILayout.HelpBox("Ale wpierdol obiekt gdzie drop ma siedzieć!", MessageType.Warning, true);
            }
            else if (objectWithDrop != null && objectWithDrop.GetComponent<EnemyStats>() == null)
            {
                EditorGUILayout.HelpBox("Na tym obiekcie nie ma klasy która przechowuje dropy!", MessageType.Warning, true);
            }
            else
            {
                howManyItems = EditorGUILayout.IntSlider("Number of items in drop: ", howManyItems, 1, 10);

                if (GUILayout.Button("Generate " + howManyItems + " item drops"))
                {
                    newDrop = ScriptableObjectUtiity.CreateAsset<ItemDrop>(dropName);
                    newDrop.itemDropBases = new ItemDropBase[howManyItems];
                }

                if (newDrop != null)
                {
                    var newEditor = Editor.CreateEditor(newDrop);
                    newEditor.OnInspectorGUI();
                }
            }
            GUILayout.EndVertical();
        }
    }

    [CanEditMultipleObjects]
    public class ItemDropCustomInspector : Editor
    {
        private ItemDrop dropBase;
        
        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical("HelpBox");
    
            DrawDropItemFields();
            
            if (GUILayout.Button("Save changes?"))
            {
                EditorUtility.SetDirty(dropBase);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            
            GUILayout.EndVertical();
        }

        private void DrawDropItemFields()
        {
            for (int i = 0; i < dropBase.itemDropBases.Length; i++)
            {
                dropBase.itemDropBases[i].ItemToDrop = (GameObject) EditorGUILayout.ObjectField("This item will drop: ", dropBase.itemDropBases[i].ItemToDrop, typeof(GameObject), true);
                dropBase.itemDropBases[i].ItemDropChance = EditorGUILayout.FloatField("With this chance: ", dropBase.itemDropBases[i].ItemDropChance);
            }
        }
    }
}
