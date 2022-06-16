using jbzd.Common.InteractSystem;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class EscortGoal : QuestGoal
{
    [SerializeField]
    private GameObject npc;
    [SerializeField]
    private GameObject place;
    
    private Collider _collider;
    private Interaction _interaction;
    
    public override void Init()
    {
        base.Init();
        
        _interaction = place.GetComponent<Interaction>();
        _collider = place.GetComponent<Collider>();
        
        _interaction.OnInteractionObjectReach += ReachedPlace;
    }

    private void ReachedPlace(Collider collider)
    {
        if (_collider != collider && collider.gameObject.CompareTag("Npc")) return;
        
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

        npc = (GameObject)EditorGUILayout.ObjectField(
        "NPC",
        npc,
        typeof(GameObject),
        true
        );
    }
}
