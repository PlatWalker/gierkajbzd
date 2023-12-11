using System.Linq;
using jbzd.Common.Extensions;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.SavingSystem;
using jbzd.SavingSystem.SaveData;
using UnityEngine;
using Zenject;

namespace jbzd.Scenes.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class DialogueTrigger : MonoBehaviour, ISaveable
    {
        public ContainerSO dialogueToStart;
        public bool wasTriggered;
        
        private DialogueManager _dialogueManager;
        
        [Inject]
        public void Constructor(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }
        
        public void OnTriggerEnter(Collider other)
        {
            if (wasTriggered) return;
            
            wasTriggered = true;
            _dialogueManager.StartDialogue(dialogueToStart);

        }

        public void LoadData(GameData gameData)
        {
            var gameObjectParents = gameObject.GetAllParentsNames();
            var gameObjectName = gameObject.name;
            
            var saveData = gameData.dialogueTriggerSaveDatas.FirstOrDefault(data =>
                data.objectPath.SequenceEqual(gameObjectParents) && data.triggerGameObjectName == gameObjectName);

            if (saveData is null)
            {
                Debug.LogError($"Something went wrong during loading data into {name}");
                return;
            }
            
            wasTriggered = saveData.wasTriggered;
        }

        public void SaveData(ref GameData gameData)
        { 
            gameData.dialogueTriggerSaveDatas.Add(new DialogueTriggerSaveData
            {
                objectPath = gameObject.GetAllParentsNames(),
                triggerGameObjectName = gameObject.name,
                wasTriggered = wasTriggered
            });
        }
    }
}
