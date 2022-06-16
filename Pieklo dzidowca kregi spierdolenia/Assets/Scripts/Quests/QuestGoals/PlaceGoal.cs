using jbzd.Common.InteractSystem;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PlaceGoal : QuestGoal
{
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
        if (_collider != collider && collider.gameObject.CompareTag("Player")) return;
        
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
