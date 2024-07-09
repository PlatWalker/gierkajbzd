using jbzd.InteractSystem;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.QuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using jbzd.MainHero;
using jbzd.MainHero.PlayerControllers;
using jbzd.Items;

namespace jbzd
{
    public class UnlockDoorWithInventory : MonoBehaviour
    {
        private QuestManager _questManager;
        private PlayerManager _playerManager;
        private Interaction _interaction;
        private InventoryController _inventoryController;
        [SerializeField] private List<ItemSO> itemsToCheck;
        [SerializeField] private GameObject teleporter;
        [SerializeField] private GameObject interactionText;
        [SerializeField] private GameObject interactionDenialText;
        [SerializeField] private Material teleportAvailableMaterial;
        [SerializeField] private Material teleportUnavailableMaterial;
        private bool entranceAvailable;
        private MeshRenderer _mesh;

        [Inject]
        public void Constructor(QuestManager questManager, PlayerManager playerManager)
        {
            _questManager = questManager;
            _playerManager = playerManager;
        }

        public void OnInteract()
        {
            if (entranceAvailable)
            {
                teleporter.SetActive(true);
                StartCoroutine(DisableTeleportAfterAWhile());
                
            }
        }
        private void Start()
        {
            _inventoryController = _playerManager.GetComponent<InventoryController>();
            
            _mesh = GetComponent<MeshRenderer>();
            entranceAvailable = false;
            teleporter.SetActive(false);
            interactionText.SetActive(false);
            interactionDenialText.SetActive(false);
            _interaction = GetComponentInChildren<Interaction>();
            _questManager.OnQuestActivation += (Quest quest) =>
            {
                if (itemsToCheck == null) entranceAvailable = true;

                if (_inventoryController.ContainsItem(itemsToCheck[0], out var invSlots))
                {
                    foreach (var item in itemsToCheck)
                    {
                        if (!_inventoryController.ContainsItem(item, out var smth)) break;
                    }
                }


                entranceAvailable = true;
            };
            foreach (var quest in _questManager.AllQuestsFromLoadedMaps)
            {
                quest.OnTaskStarted += (_, _) =>
                {
                    if (itemsToCheck == null) entranceAvailable = true;

                    foreach (var item in itemsToCheck)
                    {
                        if (!_inventoryController.ContainsItem(item, out var invSlots)) return;
                    }
                    entranceAvailable = true;
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
            else if(_interaction.IsInRange && !entranceAvailable)
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
