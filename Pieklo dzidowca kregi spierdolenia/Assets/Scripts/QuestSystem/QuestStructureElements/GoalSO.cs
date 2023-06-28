using System.Collections.Generic;
using System.Linq;
using jbzd.Dialogues.RuntimeData;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalSO : ScriptableObject
    {
        [field:SerializeField] public List<ActorSO> ActorsData { get; set; } = new();
        [Tooltip("Jesli dodasz tutaj obiekt z dialogiem, zostanie od odpalony po skonczeniu goala. Opcjonalne pole.")]
        [field:SerializeField] public ContainerSO DialogueToStartOnGoalComplete { get; set; }
        public abstract bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors);
        protected List<T> GetComponentFromActorsList<T>(List<Actor> actors) where T : Component
        {
            var componentsFromActorsList = new List<T>();

            var actorsWithChosenComponent = actors.Where(actor =>
            {
                var isInActorSuchComponent = actor.gameObject.TryGetComponent(out T component);
                
                if(isInActorSuchComponent)componentsFromActorsList.Add(component);
                
                return isInActorSuchComponent;
            }).ToList();

            if (!actorsWithChosenComponent.Any()) Debug.LogError("There are no components of this type in actor list");
            
            return componentsFromActorsList;
        }
    }
}