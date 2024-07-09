using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using jbzd.Common.Interfaces;
using jbzd.InteractSystem;
using jbzd.MainHero;

namespace jbzd
{
    public class UnlockDoorOnTasks : MonoBehaviour, IInteractable
    {
        private QuestManager _questManager;
        private Interaction _interaction;
        [SerializeField] private List<TaskSO> tasksToCheck;
        [SerializeField] private GameObject teleporter;
        [SerializeField] private GameObject interactionText;
        [SerializeField] private GameObject interactionDenialText;
        [SerializeField] private Material teleportAvailableMaterial;
        [SerializeField] private Material teleportUnavailableMaterial;
        private bool entranceAvailable;
        private MeshRenderer _mesh;

        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }

        public void OnInteract()
        {
            if(entranceAvailable)
            {
                teleporter.SetActive(true);
                StartCoroutine(DisableTeleportAfterAWhile());
            }
        }
        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            entranceAvailable = false;
            teleporter.SetActive(false);
            interactionText.SetActive(false);
            interactionDenialText.SetActive(false);
            _interaction = GetComponentInChildren<Interaction>();
            _questManager.OnQuestActivation += (Quest quest) =>
            {
                if (tasksToCheck == null) entranceAvailable = true;

                foreach (var task in tasksToCheck)
                {
                    if (quest.ActiveTask == task) entranceAvailable = true;
                }
            };
            foreach (var quest in _questManager.AllQuestsFromLoadedMaps)
            {
                quest.OnTaskStarted += (_,_) =>
                {
                    if (tasksToCheck == null) entranceAvailable = true;

                    foreach(var task in tasksToCheck)
                    {
                        if (quest.ActiveTask == task) entranceAvailable = true;
                    }
                };
            }
        }
        private void Update()
        {
            if (entranceAvailable)
            {
                if (_mesh.material == teleportAvailableMaterial) return;
                _mesh.material = teleportAvailableMaterial;
            }
            else
            {
                if (_mesh.material == teleportUnavailableMaterial) return;
                _mesh.material = teleportUnavailableMaterial;
            }
            if (_interaction.IsInRange && entranceAvailable)
            {
                interactionText.SetActive(true);
                interactionText.transform.LookAt(Camera.main.transform);

            }
            else if (_interaction.IsInRange && !entranceAvailable)
            {
                interactionDenialText.SetActive(true);
                interactionDenialText.transform.LookAt(Camera.main.transform);
            }
            else
            {
                interactionText.SetActive(false);
                interactionDenialText.SetActive(false);
            }
        }
        IEnumerator DisableTeleportAfterAWhile()
        {
            yield return new WaitForSeconds(0.25f);
            teleporter.SetActive(false);
        }
    }
}
