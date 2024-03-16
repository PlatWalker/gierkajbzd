using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.Barks;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using jbzd.MainHero;
using UnityEngine;
using Zenject;
using jbzd.Common.InputSystem;

namespace jbzd
{
    public class Cubetriggercontroller : MonoBehaviour
    {
        private BarksTrigger _barksTrigger;
        //get the box collider
        private BoxCollider _boxCollider;
        
        private PlayerManager  _playerManager;

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        void Start()
        {
            _barksTrigger = GetComponent<BarksTrigger>();
            _boxCollider = GetComponent<BoxCollider>();
        
        }

        // Update is called once per frame
        void Update()
        {
            if(_barksTrigger != null) _barksTrigger.StopBarks();
            //if player is in box collider
            if (_boxCollider.bounds.Contains(_playerManager.transform.position))
            {
                //start barks
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("WTF");
                    //if(_barksTrigger != null)_barksTrigger.StartBarks();
                }
                //_barksTrigger.StartBarks();
                //_barksTrigger.StopBarks();
            }
            else{
                
            }
        }

        public void OnInteract()
        {
            //Debug.Log("XD");
        }

    }
}
