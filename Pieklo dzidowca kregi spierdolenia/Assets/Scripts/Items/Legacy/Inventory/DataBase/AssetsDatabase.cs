using jbzdy.Items;
using UnityEngine;
using System.Collections.Generic;
using jbzd.Items.Legacy.Inventory.Inventory;

namespace jbzdy.Inventory.Database 
{
    public class AssetsDatabase : MonoBehaviour
    {
        public List<GameObject> items;

        public SharItem FindItem(string name)
        {
            foreach (var item in items)
            {
                if (item.GetComponent<SharItem>().itemName == name)
                {
                    return item.GetComponent<SharItem>();
                }
            }

            print("Find item with arg: " + name + " Item not found in database");
            return null;
        }
    }
}
