using System.Collections.Generic;
using UnityEngine;

namespace jbzd.dialogues
{
    public class ContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public Dictionary<GroupSO, List<DialogueSO>> Groups { get; set; }
        [field: SerializeField] public List<DialogueSO> Dialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new Dictionary<GroupSO, List<DialogueSO>>();
            Dialogues = new List<DialogueSO>();
        }
    }
}
