using jbzdy.Items;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Napisane przez sharashino
/// 
/// Skrypt odpowiadający za slot w inventory
/// </summary>
public class InventorySlot : MonoBehaviour
{

    [SerializeField] private Image itemIcon = default;
    [SerializeField] private Button removeButton = default;
    private Item slotItem;

    public void AddItem(Item itemToAdd)
    {
        slotItem = itemToAdd;
        itemIcon.sprite = slotItem.ItemIcon;
        itemIcon.enabled = true;
        removeButton.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        slotItem = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        removeButton.gameObject.SetActive(false);
    }

    public void OnRemoveButton()
    {
        Inventory.instance.RemoveItem(slotItem);
    }

    public void UseItem()
    {
        if(slotItem != null)
        {
            slotItem.Use();
        }
    }
}
