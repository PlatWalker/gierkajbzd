using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    public class Actor : MonoBehaviour
    {
        [field:SerializeField]
        public ActorSO ActorData { get; set; }
    }
}