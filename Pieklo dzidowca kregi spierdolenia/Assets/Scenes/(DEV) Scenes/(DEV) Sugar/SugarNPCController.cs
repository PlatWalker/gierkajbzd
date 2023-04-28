using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.Barks;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using UnityEngine;
using Zenject;

namespace jbzd
{
    public class SugarNPCController : MonoBehaviour , IInteractable
    {
        private DialogueManager _manager;
        private BarksTrigger _barksTrigger;
        [SerializeField] private ContainerSO data;
        
        [Inject]
        public void Construct(DialogueManager manager)
        {
            _manager = manager;
        }

        public void Start()
        {
            _barksTrigger = GetComponent<BarksTrigger>();
        }

        public void OnInteract()
        {
            if (data != null)
            {
                _manager.StartDialogue(data);
                
                if (_barksTrigger != null)
                {
                    _barksTrigger.StartBarks();
                    _barksTrigger.StopBarks();
                }
            }
        }
    }
}
