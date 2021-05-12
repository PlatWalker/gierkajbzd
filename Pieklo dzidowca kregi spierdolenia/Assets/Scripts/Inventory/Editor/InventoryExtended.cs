using UnityEngine;
using UnityEditor;
using jbzdy.Inventory.SaveLoad;

namespace jbzdy.Inventory.Editors
{
    [CustomEditor(typeof(InventoryClass))]
    public class InventoryExtended : Editor
    {
        InventoryClass inventory;

        private void OnEnable()
        {
            inventory = FindObjectOfType<InventoryClass>();
        }

        public override void OnInspectorGUI()
        {
            Editor editor = Editor.CreateEditor(inventory);
            editor.DrawDefaultInspector();

            if (GUILayout.Button("Clear scene persistent data"))
            {
                FindObjectOfType<SaveData>().ClearScenePersistence();
            }
        }
    }
}