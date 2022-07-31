using UnityEngine;
using jbzdy.Items;
using jbzdy.DialogueSystem.NodeDatas;
using jbzdy.Inventory;

namespace jbzdy.DialogueSystem
{
    [System.Serializable]
    [CreateAssetMenu(menuName ="Dialogue/New OpenShop Event", fileName = "OpenShop Event")]
    public class EventGetItemShop : DialogueEventSO
    {
        [SerializeField] private Item itemToGet;
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

