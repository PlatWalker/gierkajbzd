using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.QuestSystem.QuestStructureElements;
using jbzdy.Enemies;
using JetBrains.Annotations;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Kill Enemies Goal", fileName = " New KillEnemiesGoal")]
    public class KillEnemiesGoal : GoalInvolvingCollectingSO
    {
        [RunMethod]
        [UsedImplicitly]
        public void ExecuteGoalScenario(List<Actor> actors, Quest questWithThisGoal)
        {
            var enemiesControllers= GetComponentFromActorsList<EnemyController>(actors);

            foreach (var enemyController in enemiesControllers)
            {
                enemyController.OnDeath += _ =>
                {
                    var index = questWithThisGoal.GoalName.IndexOf(name);
                    questWithThisGoal.GoalItemCollected[index] += 1;
                    questWithThisGoal.CheckFinishCondition(this, actors);
                };
            }
        }

        public override bool GoalEndCondition(Quest questWithThisGoal, List<Actor> actors)
        {
            var index = questWithThisGoal.GoalName.IndexOf(name);
            
            Debug.Assert(questWithThisGoal.GoalItemCollected[index] <= GoalCompletionItemCount, 
                "GoalItemCollected property out of 'index' of max items collected");
            
            return questWithThisGoal.GoalItemCollected[index] == GoalCompletionItemCount;
        }
    }
}