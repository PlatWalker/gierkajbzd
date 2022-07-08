using jbzdy.Enemies;
using UnityEditor;
using UnityEngine;

namespace jbzd.Quests.QuestGoals
{
    [System.Serializable]
    public class KillGoal : QuestGoal
    {
        public GameObject enemy;

        public override void InGameInit()
        {
            EnemyController.OnDeath += EnemyDied;
        }

        void EnemyDied(EnemyController enemyType)
        {
            string enemyName = enemyType.name.Split(new char[] { ' ' })[0];

            if (enemyName == enemy.name)
            {
                this.CurrentAmount++;         
            
                if (IsReached())
                {
                    EnemyController.OnDeath -= EnemyDied;
                }


            }
        }
#if UNITY_EDITOR
        public override void GoalCustomEditor()
        {
            base.GoalCustomEditor();

            enemy = (GameObject)EditorGUILayout.ObjectField(
                "Przeciwnik",
                enemy,
                typeof(GameObject),
                false
            );
        }
#endif
    }
}
