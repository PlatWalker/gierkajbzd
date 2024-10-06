using UnityEngine;

namespace jbzd.Items
{
    [CreateAssetMenu(fileName = "new item", menuName = "Item")]
    public class ItemSO : ScriptableObject //ItemSO need to be in resource folder so it can be found during runtime in built game
    {
        [field:SerializeField] 
        public string ItemName { get; set; }
        
        [field:TextArea]
        [field:SerializeField]
        public string ItemDescription { get; set; }

        [field:SerializeField]
        public ItemTypes ItemType{ get; set; }

        [field: SerializeField] 
        public Sprite ItemIcon { get; set; }

        [field:SerializeField]
        public GameObject ItemPrefab { get; set; }

        [field:SerializeField]
        public int MaxStackSize { get; private set; }
    }
}
