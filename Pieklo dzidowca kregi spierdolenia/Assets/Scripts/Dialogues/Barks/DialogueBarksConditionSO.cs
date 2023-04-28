using jbzd.Common.RunnerThing;
using jbzd.Dialogues.RuntimeData;
using UnityEngine;

namespace jbzd.Dialogues.Barks
{
    [CreateAssetMenu(menuName = "Barks/Dialogue Condition", fileName = "New Dialogue Condition")]
    public class DialogueBarksConditionSO : BarksCondintionSO
    {
        [field:SerializeField] private ContainerSO dialogue;
        [field:SerializeField] private string groupName;
        
        [RunMethod]
        public void Run(DialogueManager manager)
        {
            isFulfilled = dialogue.CurrentGroup == groupName;
        }
    }
}
