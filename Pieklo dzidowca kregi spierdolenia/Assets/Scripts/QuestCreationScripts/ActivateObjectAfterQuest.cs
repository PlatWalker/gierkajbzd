using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class ActivateObjectAfterQuest : MonoBehaviour
    {
        private QuestManager _questManager;
        [SerializeField] private Quest questToCheck;
        [SerializeField] private GameObject objectToActivate;

        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        private void Start()
        {
            foreach (var quest in _questManager.AllQuestsFromLoadedMaps)
            {
                quest.onQuestEnded += (questEnded) =>
                {
                    if (questToCheck == null) return;
                    if (questEnded != questToCheck) return;
                    objectToActivate.SetActive(true);
                };
            }
        }
    }
}
