using UnityEngine;

namespace jbzdy.Items.Drop
{
    [CreateAssetMenu(menuName = "Item Drop", fileName = "New Drop")]
    public class ItemDrop : ScriptableObject
    {
        public ItemDropBase[] itemDropBases;
    }

}

