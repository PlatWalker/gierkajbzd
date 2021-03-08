using UnityEngine;

/// <summary>
/// Napisane przez sharashino  
/// 
/// Skrypt odpowiadający za szatę graficzną UI  
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsParent = default;
    [SerializeField] private GameObject inventoryUI = default;
    private Inventory inventory;
    private InventorySlot[] inventorySlots;

    // Start is called before the first frame update
    private void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    private void UpdateUI()
    {
        Debug.Log("updating ui");

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if(i < inventory.inventoryItems.Count)
            {
                inventorySlots[i].AddItem(inventory.inventoryItems[i]);
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }
    }
}
