using System;
using System.Collections.Generic;
using jbzd.Items;

namespace jbzd.SavingSystem.SaveData
{
    [Serializable]
    public class InventoryControllerSaveData
    {
        public List<InventorySlot> InventorySlots = new();
        public ItemSO headArmor;
        public ItemSO chestArmor;
        public ItemSO legArmor;
        public ItemSO bootsArmor;
        public ItemSO weapon;
    }
}