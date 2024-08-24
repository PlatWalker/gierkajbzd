using System.Collections;
using jbzd.Common.Interfaces;
using jbzd.MinorSystems.Barks;
using jbzd.MinorSystems.Interact;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    public class ShowBarkForTimeOnTaskOnInteraction: MonoBehaviour, IInteractable
    {
        [SerializeField]
        private TaskSO taskSo;

        [SerializeField]
        private int lengthOfShowingBark;
        
        private PlayerThoughtBarkController _barkController;
        private Interaction _interaction;
        
        private void Awake()
        {
            _barkController = GetComponentInChildren<PlayerThoughtBarkController>();
            _interaction = GetComponentInChildren<Interaction>();
            
            Debug.Assert(_barkController, $"Missing {nameof(PlayerThoughtBarkController)} in {gameObject.name}");
            Debug.Assert(_interaction, $"Missing {nameof(Interaction)} in {gameObject.name}");
        }
        
        public void OnInteract()
        {
            _barkController.ShowPlayerThoughtBark();

            StartCoroutine(TurnOffBark());
        }

        private IEnumerator TurnOffBark()
        {
            yield return new WaitForSeconds(lengthOfShowingBark);
            _barkController.HidePlayerThoughtBark();
            yield return null;
        }
    }
}