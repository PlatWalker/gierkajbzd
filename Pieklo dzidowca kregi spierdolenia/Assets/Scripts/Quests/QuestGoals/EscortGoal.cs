using jbzdy.Actions.Interaction;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class EscortGoal : QuestGoal
{
    public GameObject npc;
    public GameObject place;

    public override void Init()
    {
        base.Init();
        InteractionZone.OnNpcPlaceReach += ReachedPlace;
    }

    void ReachedPlace(GameObject gameObject)
    {
        if (place == gameObject)
        {
            currentAmount++;

            if (IsReached())
            {
                InteractionZone.OnNpcPlaceReach -= ReachedPlace;
            }


        }
    }

    public override void GoalCustomEditor()
    {
        base.GoalCustomEditor();

        place = (GameObject)EditorGUILayout.ObjectField(
        "Miejsce",
        place,
        typeof(GameObject),
        true
        );

        npc = (GameObject)EditorGUILayout.ObjectField(
        "NPC",
        npc,
        typeof(GameObject),
        true
        );
    }
}
