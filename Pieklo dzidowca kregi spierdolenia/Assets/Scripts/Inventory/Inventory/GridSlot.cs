using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa siedząca na slocie w Inventory
/// 
/// Napisane przez Sharashino
/// </summary>
namespace jbzdy.Inventory
{
    public class GridSlot : MonoBehaviour
    {
        /// <summary>
        /// Is this slot free
        /// </summary>
        [HideInInspector]
        public bool free;

        //Slot coordinates in inventory grid 
        [HideInInspector]
        public int x, y;
        
        /// <summary>
        /// Image component of the slot
        /// </summary>
        [HideInInspector]
        public Image image;
        
        /// <summary>
        /// If slot is for equipment it will be hold EquipmentPanel reference to exchange data about slot state
        /// </summary>
        [HideInInspector]
        public EquipmentPanel equipmentPanel;

        public bool isLoot;

        private void OnEnable()
        {
            if (image == null)
                image = GetComponent<Image>();
        }
    }
}