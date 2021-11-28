using UnityEngine;
using UnityEditor;
using jbzdy.Actions.Interaction;

[System.Serializable]
public class PlaceGoal : QuestGoal
{
    public GameObject place;

    public override void Init()
    {
        base.Init();
        InteractionZone.OnPlaceReach += ReachedPlace;
    }

    void ReachedPlace(GameObject gameObject)
    {
        if (place == gameObject)
        {
            currentAmount++;

            if (IsReached())
            {
                InteractionZone.OnPlaceReach -= ReachedPlace;
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
    }

}
