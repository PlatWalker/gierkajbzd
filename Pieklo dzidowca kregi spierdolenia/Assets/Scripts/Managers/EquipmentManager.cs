using jbzdy.Items;
using jbzdy.Items.Enums;
using UnityEngine;

/// <summary>
/// Napisane przez sharashino
/// 
/// Manager tego co gracz ma aktualnie zaekwipowane
/// 
/// W niedalekiej przyszłości będzie odpowiadać za logike ekwipowania itemów
/// </summary>
namespace jbzdy.Managers
{
    public class EquipmentManager : MonoBehaviour
    {
        #region singleton
        public static EquipmentManager instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        private EquipableItem[] currentEquipment;
        private Inventory inventory;

        public delegate void OnEquipmentChange(EquipableItem itemToEquip, EquipableItem itemToRemove);
        public OnEquipmentChange onEquipmentChange;

        private void Start()
        {
            inventory = Inventory.instance;

            int slotsNumber = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new EquipableItem[slotsNumber];
        }

        public void EquipItem(EquipableItem itemToEquip)
        {
            int slotIndex = (int)itemToEquip.equipmentSlot;

            EquipableItem oldItem = null;

            if (currentEquipment[slotIndex] != null)
            {
                oldItem = currentEquipment[slotIndex];
                inventory.AddItem(oldItem);
            }

            if (onEquipmentChange != null)
            {
                onEquipmentChange.Invoke(itemToEquip, oldItem);
            }

            currentEquipment[slotIndex] = itemToEquip;
        }

        public void UnequipItem(int itemIndex)
        {
            if (currentEquipment[itemIndex] != null)
            {
                EquipableItem oldItem = currentEquipment[itemIndex];
                inventory.AddItem(oldItem);

                currentEquipment[itemIndex] = null;

                if (onEquipmentChange != null)
                {
                    onEquipmentChange.Invoke(null, oldItem);
                }
            }
        }
    }

}
