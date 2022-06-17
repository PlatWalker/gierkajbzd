using jbzd.Common.InteractSystem;
using UnityEditor;
using UnityEngine;

namespace jbzd.Quests.QuestGoals
{
    [System.Serializable]
    public class PlaceGoal : QuestGoal
    {
        [SerializeField]
        private GameObject place;
    
        private Interaction _interaction;
    
        public override void Init()
        {
            base.Init();
        
            _interaction = place.GetComponent<Interaction>();

            _interaction.OnInteractionObjectReach += ReachedPlace;
        }

        private void ReachedPlace(Collider collider)
        {
            if (!collider.gameObject.CompareTag("Player")) return;
        
            currentAmount++;

            if (IsReached()) _interaction.OnInteractionObjectReach -= ReachedPlace;
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
}
