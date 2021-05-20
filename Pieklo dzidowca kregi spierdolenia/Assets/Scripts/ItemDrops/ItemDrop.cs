using UnityEngine;

namespace jbzdy.Items.Drop
{
    [CreateAssetMenu(menuName = "Item Drop", fileName = "New Drop")]
    public class ItemDrop : ScriptableObject
    {
        [SerializeField] public ItemDropBase[] itemDropBases;
    }
}

