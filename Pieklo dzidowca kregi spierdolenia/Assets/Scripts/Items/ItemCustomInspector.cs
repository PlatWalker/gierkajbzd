    using jbzdy.Items;
using jbzdy.Items.Enums;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace jbzdy.Inventory.Editors
{
    [CustomEditor(typeof(Item))]
    public class ItemCustomInspector : Editor
    {
        private Item item;

        public override void OnInspectorGUI()
        {
            item = target as Item;
            
            if(item) DrawGeneralItem();
            
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.red;
            style.fontStyle = FontStyle.Bold;

            if (GUILayout.Button("Save changes?", style))
            {
                EditorUtility.SetDirty(item);
                EditorSceneManager.MarkSceneDirty(item.gameObject.scene);
            }
        }

        public void DrawGeneralItem()
        {
            GUILayout.Label("General item settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox", GUILayout.Width(300));
            item.itemName = EditorGUILayout.TextField("Name", item.itemName);
            item.itemDescription = EditorGUILayout.TextField("Description", item.itemDescription);

            item.itemIcon = (Sprite)EditorGUILayout.ObjectField("Item icon", item.itemIcon, typeof(Sprite), false);
            item.inventoryItemSprite = (Sprite)EditorGUILayout.ObjectField("Item icon in inventory", item.inventoryItemSprite, typeof(Sprite), false);
            item.itemID = EditorGUILayout.IntField("ID", item.itemID);

            if (GUILayout.Button("Generate random ID?"))
            {
                item.itemID = Random.Range(0, int.MaxValue);
            }

            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Item grid size", EditorStyles.boldLabel);
            item.itemWidth = EditorGUILayout.IntField("Width", item.itemWidth);
            item.itemHeight = EditorGUILayout.IntField("Height", item.itemHeight);
            GUILayout.EndVertical();
            
                item.isStackable = EditorGUILayout.Toggle("Stackable", item.isStackable);

                if (item.isStackable)
                {
                    item.itemStackSize = EditorGUILayout.IntSlider("Item stack size", item.itemStackSize, 1, 100);
                    item.itemMaxStackSize = EditorGUILayout.IntSlider("Max stack size", item.itemMaxStackSize, 1, 100);
                }
            
            GUILayout.EndVertical();
        }
    }
}
