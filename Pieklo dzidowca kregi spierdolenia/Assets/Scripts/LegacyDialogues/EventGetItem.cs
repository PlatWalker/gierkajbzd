using jbzd.Items.Legacy.Inventory.Inventory;
using jbzd.LegacyDialogues.NodesDatas;
using UnityEngine;
using jbzdy.Items;
using jbzdy.Inventory;

namespace jbzdy.DialogueSystem
{
    [System.Serializable]
    [CreateAssetMenu(menuName ="Dialogue/New OpenShop Event", fileName = "OpenShop Event")]
    public class EventGetItemShop : DialogueEventSO
    {
        [SerializeField] private SharItem itemToGet;
        public override void RunEvent()
        {
            base.RunEvent();
            GetItem();
        }

        private void GetItem()
        {
            InventoryClass.Instance.AddItem(itemToGet);
        }
    }
}

