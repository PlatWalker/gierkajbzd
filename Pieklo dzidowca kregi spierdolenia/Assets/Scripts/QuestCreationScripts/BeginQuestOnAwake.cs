using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            SceneManager.sceneUnloaded += _ =>
            {
                SceneManager.sceneLoaded -= StartQuest;
            };
        }

        private void StartQuest(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.Equals(gameObject.scene))
            {
                foreach (var quest in _questManager.ActiveQuests)
                {
                    if (quest == questToStart) return;
                }
                if (questToStart.IsCompleted) return;
                _questManager.StartQuest(questToStart);
            }
        }
    }
}
