using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using jbzd.Dialogues;
using jbzd.Enemies;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.UI;
using jbzd.UI.Dialogues;
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
        public void ExecuteGoalScenario(List<Actor> actors, Quest questWithThisGoal, UserInterfaceManager uiManager)
        {
            var enemiesControllers= GetComponentFromActorsList<EnemyController>(actors);
            var dialogueController = uiManager.GetUIController<DialogueUIController>();

            foreach (var enemyController in enemiesControllers)
            {
                enemyController.OnDeath += _ =>
                {
                    Debug.Log($"Enemy {enemyController.gameObject} has died and goal {name} has registered it");
                    var index = questWithThisGoal.GoalName.IndexOf(name);
                    questWithThisGoal.GoalItemCollected[index] += 1;
                    questWithThisGoal.CheckFinishCondition(this, actors);
                    
                    if (DialogueToStartOnGoalComplete is not null &&
                        GoalEndCondition(questWithThisGoal, actors))
                    {
                        dialogueController.StartDialogue(DialogueToStartOnGoalComplete);
                    }
                    
                };
            }
            
            var enemiesControllersNew= GetComponentFromActorsList<Enemy>(actors);
            
            foreach (var enemyController in enemiesControllersNew)
            {
                enemyController.OnDeath += () =>
                {
                    Debug.Log($"Enemy {enemyController.gameObject} has died and goal {name} has registered it");
                    var index = questWithThisGoal.GoalName.IndexOf(name);
                    questWithThisGoal.GoalItemCollected[index] += 1;
                    questWithThisGoal.CheckFinishCondition(this, actors);
                    
                    if (DialogueToStartOnGoalComplete is not null &&
                        GoalEndCondition(questWithThisGoal, actors))
                    {
                        dialogueController.StartDialogue(DialogueToStartOnGoalComplete);
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