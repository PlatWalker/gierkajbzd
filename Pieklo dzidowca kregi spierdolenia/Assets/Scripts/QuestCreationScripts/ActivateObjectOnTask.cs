using jbzd.Dialogues.RuntimeData;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace jbzd
{
    public class ActivateObjectOnTask : MonoBehaviour
    {
        private QuestManager _questManager;
        [SerializeField] private Quest questToCheck;
        [SerializeField] private TaskSO taskToCheck;
        [SerializeField] private GameObject objectToActivate;

        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        private void Start()
        {
            questToCheck.onTaskUpdated += (object sender, EventArgs e) =>
            {
                if (questToCheck == null) return;
                if (taskToCheck == null) return;
                if (questToCheck.ActiveTask == taskToCheck)
                {
                    objectToActivate.SetActive(true);
                }
            };
        }
    }
}
