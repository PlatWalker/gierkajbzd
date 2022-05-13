using UnityEngine;
using UnityEditor;
using jbzdy.Items;
using jbzdy.Inventory;

[System.Serializable]
public class ItemGoal : QuestGoal
{
    public Item item;
    private InventoryClass inventory;

    public override void Init()
    {
        base.Init();
        inventory = InventoryClass.Instance;
        inventory.OnInventoryItemAdd.AddListener(CheckItemState);
        inventory.OnInventoryItemRemove.AddListener(CheckItemState);

        if (inventory.TakeItemFromPlayer(item, requiredAmount, false)) CheckItemState();
    }

    void CheckItemState()
    {
        currentAmount = inventory.CheckItemAmount(item);

        if (!IsReached() && Completed)  //jesli ktos mial wszystkie itemki, ale np. wyrzucil
        {
            Completed = false;
        }


    }

    public override void GoalCustomEditor()
    {
        base.GoalCustomEditor();

        item = (Item)EditorGUILayout.ObjectField(
        "Przedmiot",
        item,
        typeof(Item),
        true
        );
    }

    public void RemoveListeners()
    {
        inventory.OnInventoryItemAdd.RemoveListener(CheckItemState);
        inventory.OnInventoryItemRemove.RemoveListener(CheckItemState);
    }

}
