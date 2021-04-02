using jbzdy.Items;
using UnityEngine;
using System.Collections.Generic;

namespace jbzdy.Inventory.Database 
{

    public class AssetsDatabase : MonoBehaviour
    {
        public List<GameObject> items;

        public Item FindItem(string name)
        {
            foreach (var item in items)
            {
                if (item.GetComponent<Item>().itemName == name)
                {
                    return item.GetComponent<Item>();
                }
            }

            print("Find item with arg: " + name + " Item not found in database");
            return null;
        }
    }
}
