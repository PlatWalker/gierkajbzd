using UnityEngine;
using jbzdy.Enemies;
using UnityEditor;

[System.Serializable]
public class KillGoal : QuestGoal
{
    public GameObject enemy;

    public override void Init()
    {
        base.Init();
        EnemyController.OnDeath += EnemyDied;
    }

    void EnemyDied(EnemyController enemyType)
    {
        string enemyName = enemyType.name.Split(new char[] { ' ' })[0];

        if (enemyName == enemy.name)
        {
            this.currentAmount++;         
            
            if (IsReached())
            {
                EnemyController.OnDeath -= EnemyDied;
            }

            Debug.Log(currentAmount + "/" + requiredAmount);
        }
    }

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

}
