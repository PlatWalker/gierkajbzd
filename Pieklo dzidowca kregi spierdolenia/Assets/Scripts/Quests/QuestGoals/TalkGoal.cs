using jbzdy.Actions.Interaction;
using UnityEditor;
using UnityEngine;

namespace jbzd.Quests.QuestGoals
{
    [System.Serializable]
    public class TalkGoal : QuestGoal
    {
        public GameObject npc;

        public override void InGameInit()
        {
            Interactable.OnInteraction += InteractionHappend;
        }

        void InteractionHappend(GameObject gameObject)
        {
            if(npc == gameObject)
            {
                this.CurrentAmount++;

                if (IsReached())
                {
                    Interactable.OnInteraction -= InteractionHappend;
                }


            }
        }
#if UNITY_EDITOR
        
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
#endif
    }
}
