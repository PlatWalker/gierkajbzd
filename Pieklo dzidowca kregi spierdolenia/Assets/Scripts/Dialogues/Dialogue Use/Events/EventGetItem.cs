using UnityEngine;
using jbzdy.Items;
using jbzdy.DialogueSystem.SO;
using jbzdy.Inventory;

/// <summary>
/// Napisane przez sharashino
/// 
/// Event w dialogu odpowiadający za otrzymanie itemu
/// </summary>
namespace jbzdy.DialogueSystem.Events
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

