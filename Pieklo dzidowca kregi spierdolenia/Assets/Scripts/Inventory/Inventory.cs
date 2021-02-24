using jbzdy.Items;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Napisane przez Sharashino
/// 
/// Skrypt odpowiadający za nasze inventory, dodawanie, usuwanie operacje na inventory
/// </summary>
public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();   //Delegate 
    public OnItemChanged onItemChangedCallback; //Kiedy coś zmienia sie w naszym inventory wywołujemy tą funkcję żeby wiedzieć kiedy updatować ui

    public List<Item> inventoryItems = new List<Item>();
    [SerializeField] private int inventorySpace = 20;


    public bool AddItem(Item itemToAdd)
    {
        if(inventoryItems.Count >= inventorySpace)
        {
            Debug.Log("Not enough inventory space!");
            return false;
        }

        inventoryItems.Add(itemToAdd);
        
        if(onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return true;
    }

    public void RemoveItem(Item itemToRemove)
    {
        inventoryItems.Remove(itemToRemove);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}


