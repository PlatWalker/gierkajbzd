using UnityEngine;

/// <summary>
/// Made by sharashino
/// 
/// Każdy obiekt który gracz może skonsumować powinien mieć tą klase
/// </summary>
public class ItemConsume : Interactables
{
    [SerializeField] ConsumableItem consumable;

    public override void Interact()
    {
        base.Interact();
        ConsumeItem();
    }

    private void ConsumeItem()
    {
        Debug.Log("Consuming " + consumable.itemName);

        //Destroy(gameObject);
    }
}
