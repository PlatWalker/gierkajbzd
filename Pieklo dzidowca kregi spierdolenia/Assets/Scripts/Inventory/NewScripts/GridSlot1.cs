using UnityEngine;
using UnityEngine.UI;
using jbzdy.Inventory.Equipment;

namespace jbzdy.Inventory
{
    public class GridSlot1 : MonoBehaviour
    {
        public bool free;
        public int x, y;
        public Image slotImage;
        public EquipmentPanel1 equipmentPanel;

        public bool isLoot;

        private void OnEnable()
        {
            if(slotImage == null)
            {
                slotImage = GetComponent<Image>();
            }
        }
    }
}
