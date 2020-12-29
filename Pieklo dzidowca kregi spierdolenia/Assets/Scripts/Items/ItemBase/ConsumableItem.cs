using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableItem", menuName ="Item/New Consumable Item")]
public class ConsumableItem : ScriptableObject
{
    public string itemName;
    public string itemDesc;
    public GameObject itemObject;

    public void ConsumeItem()
    {
        Debug.Log("Item" + itemName + " consumed");
    }
}
