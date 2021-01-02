using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName ="Items/New Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public string itemDesc = "It must be awesome";
    public Sprite itemIcon = null;
    public GameObject itemObject;
    public bool canPickUp;
}
