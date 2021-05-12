using jbzdy.Items;
using UnityEngine;
using System.Collections.Generic;

namespace jbzdy.Inventory.Database
{
    [CreateAssetMenu(fileName = "New items database")]
    public class ItemDatabase : ScriptableObject
    {
        public string databaseName;
        public List<Item> items;

        public void ScanProjectItems(List<Item> items)
        {
            this.items.Clear();
            
            foreach(var item in items)
            {
                this.items.Add(item);
            }
        }

        public Item FindItem(string title)
        {
            foreach(var item in items)
            {
                if (item.itemName == title)
                    return item;
            }

            return null;
        }
    }
}