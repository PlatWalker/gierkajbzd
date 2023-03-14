using System.Collections.Generic;
using System.Linq;
using jbzd.NPC;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalSO : ScriptableObject
    {
        [field:SerializeField] public List<ActorSO> ActorsData { get; set; } = new();
        public abstract bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors);
        
        protected NpcController GetNpcControllerFromActorsList(List<Actor> actors)
        {
            NpcController npcController = null;

            var npcActor = actors.FirstOrDefault(actor => actor.gameObject.TryGetComponent(out npcController));

            if (npcActor is null) Debug.LogError("Something went wrong");
            return npcController;
        }
    }
}