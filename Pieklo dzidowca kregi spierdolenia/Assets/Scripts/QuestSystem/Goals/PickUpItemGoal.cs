using System;
using System.Collections.Generic;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEditor;
using UnityEngine;

namespace jbzd.QuestSystem.Goals
{
    [CreateAssetMenu(menuName = "Quest/Goals/Pick Up Item Goal", fileName = "New PickUpItemGoal")]
    public class PickUpItemGoal : GoalSO
    {
        
        public override void ExecuteGoalScenario(List<Actor> actors)
        {
            //TODO do zrobienia gol
            throw new NotImplementedException();
        }

        public override bool GoalEndCondition()
        {
            throw new NotImplementedException();
        }
    }
}