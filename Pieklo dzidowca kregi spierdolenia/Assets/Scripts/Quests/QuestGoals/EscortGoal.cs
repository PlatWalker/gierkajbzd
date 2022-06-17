using jbzd.Common.InteractSystem;
using jbzd.NPC;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace jbzd.Quests.QuestGoals
{
    [System.Serializable]
    public class EscortGoal : QuestGoal
    {
        [SerializeField]
        private GameObject npc;
        [SerializeField]
        private GameObject place;
    
        private Interaction _interaction;
        private NpcController _npcController;

        public override void Init()
        {
            base.Init();
            if (place.GetComponent<Interaction>() == null)
            {
                Debug.LogWarning("Punkt docelowy quest'a z eskortą musi być obiektem z interakcją!");
                return;
            }

            _npcController = npc.GetComponent<NpcController>();
            _interaction = place.GetComponent<Interaction>();

            _npcController.FollowPlayer = true;
            _interaction.OnInteractionObjectReach += ReachedPlace;
        }

        private void ReachedPlace(Collider collider)
        {
            if (collider.gameObject != npc) return;
        
            currentAmount++;

            if (IsReached())
            {
                _interaction.OnInteractionObjectReach -= ReachedPlace;
                _npcController.FollowPlayer = false;
                _npcController.MoveTo(place.transform.position + new Vector3(Random.Range(0,3),0,Random.Range(0,3)));
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
}
