using jbzdy.CharacterStats;
using jbzdy.Items.Drop;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Napisane przez Sharashino
///
/// Klasa odpowiadająca za statystyki u przeciwników
/// Obecnie jeszcze zawiera funkcję do spawnowania itemów
/// </summary>
public class EnemyStats : CharacterStats
{
    [SerializeField] private ItemDrop enemyItemDrop;
    public bool die;
    
    public ItemDrop EnemyItemDrop
    {
        get => enemyItemDrop;
        set => enemyItemDrop = value;
    }

    private void Update()
    {
        if (die)
        {
            Health.BaseValue = 0;
            CharacterDie();
            die = false;
        }
    }

    protected override void CharacterDie()
    {
        DropItems();
    }

    private void DropItems()
    {
        Random generator = new Random();
        
        foreach (ItemDropBase item in enemyItemDrop.itemDropBases)
        {
            float propability = item.ItemDropChance;
            int randomChance = generator.Next(0, 100);

            if (randomChance < propability)
            {
                GameObject newItem = Instantiate(item.ItemToDrop);
                var position = transform.position;
                newItem.transform.position = new Vector3(position.x + generator.Next(-5,5), 0.5f, position.z + generator.Next(-5,5));
            }
        }
    }
}
