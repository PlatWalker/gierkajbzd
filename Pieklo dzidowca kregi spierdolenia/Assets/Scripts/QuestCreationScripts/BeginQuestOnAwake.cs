using System;
using System.Linq;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace jbzd.QuestCreationScripts
{
    public class BeginQuestOnAwake : MonoBehaviour
    {
        [SerializeField] private Quest questToStart;
        
        private QuestManager _questManager;
        
        [Inject]
        public void Constructor(QuestManager questManager)
        {
            _questManager = questManager;
        }
        
        private void Awake()
        {
            SceneManager.sceneLoaded += StartQuest;
        }

        private void StartQuest(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            if (!loadedScene.Equals(gameObject.scene)) return;
            
            if (_questManager.ActiveQuests.Any(quest => quest == questToStart) || questToStart.IsCompleted)
            {
                return;
            }
            
            _questManager.StartQuest(questToStart);
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= StartQuest;
        }
    }
}
