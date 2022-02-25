using UnityEngine;
using UnityEditor;
using jbzdy.Items;
using System.Collections.Generic;

namespace jbzdy.Inventory
{
    [CustomEditor(typeof(AssetDatabase))]
    public class DatabaseEditor : Editor
    {
        public List<Item> assetsItems;

        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Scan assets for items"))
            {
                assetsItems.Clear();

                var items = Resources.FindObjectsOfTypeAll<Item>();

                foreach(var _item in items)
                {
                    assetsItems.Add(_item);
                }
            }
        }
    }
}