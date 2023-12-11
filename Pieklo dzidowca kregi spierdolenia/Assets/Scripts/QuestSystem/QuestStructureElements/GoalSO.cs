using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common;
using jbzd.Dialogues.RuntimeData;
using jbzd.Enemies;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public abstract class GoalSO : ScriptableObject
    {
        [field:JbzdReadOnly]
        [field:SerializeField]
        public string Id { get; set; }
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

            if (!actorsWithChosenComponent.Any())
            {
                if (typeof(EnemyController) == typeof(T))
                {
                    Debug.LogWarning($"There are no components of {nameof(EnemyController)} type in actor list in goal: {name}" + 
                                     " but enemyController will be deleted.");
                }
                else
                {
                    Debug.LogError($"There are no components of {typeof(T)} type in actor list in goal: {name}");
                }
            }
            
            return componentsFromActorsList;
        }
        public void OnEnable()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}