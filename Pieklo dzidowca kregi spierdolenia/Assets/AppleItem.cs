using UnityEngine;

/// <summary>
/// Made by Sharashino
/// </summary>
public class AppleItem : MonoBehaviour
{
    [SerializeField] private ConsumableItem appleItem;
    
    public ConsumableItem GetItem()
    {
        return appleItem;
    }
}
