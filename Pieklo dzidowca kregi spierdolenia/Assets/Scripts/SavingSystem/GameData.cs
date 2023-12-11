using System;
using System.Collections.Generic;
using jbzd.SavingSystem.SaveData;
using UnityEngine;

namespace jbzd.SavingSystem
{
    [Serializable]
    public class GameData
    {
        public Vector3 playerPosition;
        public QuestManagerSaveData questManagerSaveData;

        public List<DialoguesSaveData> dialoguesSaveDatas = new();
        public List<DialogueTriggerSaveData> dialogueTriggerSaveDatas = new();
        public List<QuestSaveData> questSaveDatas = new();
        public List<string> openedScenes = new();
        public List<DisabledAndDestroyedGameObjectsSaveData> gameObjectsStatusSaveDatas = new();
    }
}