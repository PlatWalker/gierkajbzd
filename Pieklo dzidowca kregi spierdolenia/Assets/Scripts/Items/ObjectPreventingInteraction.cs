using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Enemies;
using jbzd.Items;
using UnityEngine;

namespace jbzd
{
    [RequireComponent(typeof(BoxCollider))]
    public class ObjectPreventingInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _collidingObject;
        private Enemy _object;
        private QuestItem _item;
        
        
        private void Awake()
        {
            _item = GetComponent<QuestItem>();
            _object = _collidingObject.GetComponent<Enemy>();
            _object.OnDeath += OnObjectDestroyed; 
            _item.isInteractable = false;
        }
        private void OnObjectDestroyed()
        {
            _item.isInteractable = true;
        }
        private void OnDestroy()
        {
            _object.OnDeath -= OnObjectDestroyed;
        }
    }
}
