using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalSO : ScriptableObject
    {
        [field:SerializeField] public List<ActorSO> ActorsData { get; set; } = new();
        public abstract bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors);
        protected List<T> GetComponentFromActorsList<T>(List<Actor> actors) where T : Component
        {
            var componentsFromActorsList = new List<T>();

            var actorsWithChosenComponent = actors.Where(actor =>
            {
                var isInActorSuchComponent = actor.gameObject.TryGetComponent(out T component);
                componentsFromActorsList.Add(component);
                return isInActorSuchComponent;
            }).ToList();

            if (!actorsWithChosenComponent.Any()) Debug.LogError("There are no components of this type in actor list");
            
            return componentsFromActorsList;
        }
    }
}