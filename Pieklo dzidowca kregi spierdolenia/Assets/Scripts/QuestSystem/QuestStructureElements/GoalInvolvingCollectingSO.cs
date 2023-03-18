using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalInvolvingCollectingSO : GoalSO
    {
        [field:SerializeField]
        public int GoalCompletionItemCount { get; private set; }
    }
}