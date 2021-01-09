using UnityEngine;

/// <summary>
/// Napisane przez Sharashino
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName ="Items/New Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public string itemDesc = "It must be awesome";
    public Sprite itemIcon = null;
    public GameObject itemObject;
    public bool canPickUp;

    public virtual void Use()
    {
        //Using the default Item
        //
        //ja tak jak kot siedem żyć i baluje całą noc

        Debug.Log("Using " + itemName);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.RemoveItem(this);
    }
}

public enum StatModifierType
{
    Health,
    Mana,
    Armor,
    Strenght,
    Agility,
    Intelligence,
    Vitality,
    Luck
}