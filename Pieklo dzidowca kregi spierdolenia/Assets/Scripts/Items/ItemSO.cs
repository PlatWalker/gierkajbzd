using UnityEngine;

namespace jbzd.Items
{
    [CreateAssetMenu(fileName = "new item", menuName = "Item")]
    public class ItemSO : ScriptableObject
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

        [field:Range(1,10)]
        [field:SerializeField]
        public int GridWidth { get; private set; }
        [field:Range(1,10)]
        [field:SerializeField]
        public int GridHeight { get; private set; }
    }
}
