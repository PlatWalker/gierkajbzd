using System;
using jbzd.Common;
using UnityEngine;

namespace jbzd.QuestSystem.QuestStructureElements
{
    [CreateAssetMenu(menuName = "Quest/Actor", fileName = "New Actor Data")]
    public class ActorSO : ScriptableObject
    {
        [field:JbzdReadOnly]
        [field:SerializeField]
        public string Id { get; set; }
        
        public void OnEnable()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}