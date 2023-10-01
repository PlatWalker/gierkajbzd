using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using jbzd.Dialogues;
using jbzd.Dialogues.Barks;
using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.UI;
using UnityEngine;
using Zenject;

namespace jbzd
{
    public class BarksSimpleController : MonoBehaviour , IInteractable
    {
        private BarksTrigger _barksTrigger;
        public void Start()
        {
            _barksTrigger = GetComponent<BarksTrigger>();
        }

        public void OnInteract()
        {
            if (_barksTrigger != null)
            {
                _barksTrigger.StartBarks();
            }
        }
    }
}