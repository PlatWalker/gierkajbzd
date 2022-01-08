using UnityEngine;
using UnityEditor;
using jbzdy.Actions.Interaction;

[System.Serializable]
public class TalkGoal : QuestGoal
{
    public GameObject npc;

    public override void Init()
    {
        base.Init();
        Interactable.OnInteraction += InteractionHappend;
    }

    void InteractionHappend(GameObject gameObject)
    {
        if(npc == gameObject)
        {
            this.currentAmount++;

            if (IsReached())
            {
                Interactable.OnInteraction -= InteractionHappend;
            }


        }
    }

    public override void GoalCustomEditor()
    {
        base.GoalCustomEditor();

        npc = (GameObject)EditorGUILayout.ObjectField(
        "NPC",
        npc,
        typeof(GameObject),
        true
        );
    }

}
