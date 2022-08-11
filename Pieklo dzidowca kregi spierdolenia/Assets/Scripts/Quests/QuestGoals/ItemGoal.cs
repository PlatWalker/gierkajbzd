using jbzdy.Inventory;
using jbzdy.Items;
using UnityEditor;

namespace jbzd.Quests.QuestGoals
{
    [System.Serializable]
    public class ItemGoal : QuestGoal
    {
        public SharItem item;
        private InventoryClass inventory;

        public override void InGameInit()
        {
            inventory = InventoryClass.Instance;
            inventory.OnInventoryItemAdd.AddListener(CheckItemState);
            inventory.OnInventoryItemRemove.AddListener(CheckItemState);

            if (inventory.TakeItemFromPlayer(item, RequiredAmount, false)) CheckItemState();
        }

        void CheckItemState()
        {
            CurrentAmount = inventory.CheckItemAmount(item);

            if (!IsReached() && completed)  //jesli ktos mial wszystkie itemki, ale np. wyrzucil
            {
                completed = false;
            }


        }
#if UNITY_EDITOR
        public override void GoalCustomEditor()
        {
            base.GoalCustomEditor();

            item = (SharItem)EditorGUILayout.ObjectField(
                "Przedmiot",
                item,
                typeof(SharItem),
                true
            );
        }
#endif
        public void RemoveListeners()
        {
            inventory.OnInventoryItemAdd.RemoveListener(CheckItemState);
            inventory.OnInventoryItemRemove.RemoveListener(CheckItemState);
        }

    }
}
