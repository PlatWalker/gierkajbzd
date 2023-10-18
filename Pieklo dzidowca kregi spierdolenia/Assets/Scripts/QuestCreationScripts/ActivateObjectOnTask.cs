using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class ActivateObjectOnTask : MonoBehaviour
    {
        private QuestManager _questManager;
        [SerializeField] private TaskSO taskToCheck;
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
                quest.onTaskUpdated += (object sender, EventArgs e) =>
                {
                    if (taskToCheck == null) return;
                    if (quest.ActiveTask != taskToCheck) return;
                    objectToActivate.SetActive(true);
                };
            }

        }
    }
}
