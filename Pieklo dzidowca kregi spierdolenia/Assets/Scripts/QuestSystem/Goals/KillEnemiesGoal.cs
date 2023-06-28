using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.Dialogues;
using jbzd.Enemies;
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
        public void ExecuteGoalScenario(List<Actor> actors, Quest questWithThisGoal, DialogueManager dialogueManager)
        {
            var enemiesControllers= GetComponentFromActorsList<EnemyController>(actors);

            foreach (var enemyController in enemiesControllers)
            {
                enemyController.OnDeath += _ =>
                {
                    var index = questWithThisGoal.GoalName.IndexOf(name);
                    questWithThisGoal.GoalItemCollected[index] += 1;
                    questWithThisGoal.CheckFinishCondition(this, actors);
                    
                    if (DialogueToStartOnGoalComplete is not null &&
                        GoalEndCondition(questWithThisGoal, actors))
                    {
                        dialogueManager.StartDialogue(DialogueToStartOnGoalComplete);
                    }
                };
            }
            
            var enemiesControllersNew= GetComponentFromActorsList<Enemy>(actors);
            
            foreach (var enemyController in enemiesControllersNew)
            {
                enemyController.OnDeath += () =>
                {
                    var index = questWithThisGoal.GoalName.IndexOf(name);
                    questWithThisGoal.GoalItemCollected[index] += 1;
                    questWithThisGoal.CheckFinishCondition(this, actors);
                    
                    if (DialogueToStartOnGoalComplete is not null &&
                        GoalEndCondition(questWithThisGoal, actors))
                    {
                        dialogueManager.StartDialogue(DialogueToStartOnGoalComplete);
                    }
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