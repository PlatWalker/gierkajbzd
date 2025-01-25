using jbzd.Enemies.EnemiesComponents;
using jbzd.Enemies.Obsolete;
using UnityEngine;

namespace jbzd.Items
{
    [RequireComponent(typeof(BoxCollider))]
    public class ObjectPreventingInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _collidingObject;
        private CanBeDamaged _object;
        private QuestItem _item;
        
        
        private void Awake()
        {
            _item = GetComponent<QuestItem>();
            _object = _collidingObject.GetComponent<CanBeDamaged>();
            _object.OnDeath += OnObjectDestroyed;
            _item.isInteractable = false;
        }
        private void OnObjectDestroyed(CanBeDamaged enemy)
        {
            _item.isInteractable = true;
        }
        private void OnDestroy()
        {
            _object.OnDeath -= OnObjectDestroyed;
        }
    }
}
