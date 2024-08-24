using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Common.Interfaces;
using jbzd.MinorSystems.Interact;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using Zenject;

namespace jbzd.Scenes.MapInteractionZones
{
    public class UnlockDoorOnTasks : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<TaskSO> tasksToCheck;
        [SerializeField] private GameObject teleporter;
        [SerializeField] private GameObject interactionText;
        [SerializeField] private GameObject interactionDenialText;
        [SerializeField] private Material teleportAvailableMaterial;
        [SerializeField] private Material teleportUnavailableMaterial;
        
        
        private bool _entranceAvailable;
        private MeshRenderer _mesh;
        private QuestManager _questManager;
        private Interaction _interaction;

        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
            _interaction = GetComponentInChildren<Interaction>();
        }

        private void Start()
        {
            _questManager.OnQuestActivation += (Quest quest) =>
            {
                if (tasksToCheck == null) _entranceAvailable = true;

                foreach (var task in tasksToCheck)
                {
                    if (quest.ActiveTask == task) _entranceAvailable = true;
                }
            };
            
            foreach (var quest in _questManager.AllQuestsFromLoadedMaps)
            {
                quest.OnTaskStarted += (_,_) =>
                {
                    if (tasksToCheck == null) _entranceAvailable = true;

                    foreach(var task in tasksToCheck)
                    {
                        if (quest.ActiveTask == task) _entranceAvailable = true;
                    }
                };
            }
        }
        
        public void OnInteract()
        {
            if (!_entranceAvailable) return;
            
            teleporter.SetActive(true);
            StartCoroutine(DisableTeleportAfterAWhile());
        }
        
        private void Update()
        {
            if (_entranceAvailable)
            {
                if (_mesh.material == teleportAvailableMaterial) return;
                _mesh.material = teleportAvailableMaterial;
            }
            else
            {
                if (_mesh.material == teleportUnavailableMaterial) return;
                _mesh.material = teleportUnavailableMaterial;
            }
            if (_interaction.IsInRange && _entranceAvailable)
            {
                interactionText.SetActive(true);
                interactionText.transform.LookAt(Camera.main.transform);

            }
            else if (_interaction.IsInRange && !_entranceAvailable)
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

        private IEnumerator DisableTeleportAfterAWhile()
        {
            yield return new WaitForSeconds(0.25f);
            teleporter.SetActive(false);
        }
    }
}
