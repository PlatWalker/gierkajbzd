using System.Collections.Generic;
using System.Linq;
using jbzd.Common.Interfaces;
using jbzd.Dialogues.RuntimeData;
using jbzd.Plugins.DropdownAttributes.Core.Scripts;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    public class SetDialogueGroupOnInteraction : MonoBehaviour, IInteractable
    {
        private List<string> GroupNames { get; set; } = new();
        
        [SerializeField] private ContainerSO dialogueContainer;
        
        [Dropdown(nameof(GroupNames))]
        [SerializeField] private string groupToSet;

        #if UNITY_EDITOR
        public void OnValidate()
        {
            GroupNames = new List<string>(dialogueContainer.Groups.Select(x => x.Key.GroupName));
        }
        #endif

        public void OnInteract()
        {
            if (dialogueContainer.UtilityGroup.FirstOrDefault(x => x.Key.GroupName == groupToSet).Value != null)
                dialogueContainer.CurrentGroup = groupToSet;
        }
    }
}
