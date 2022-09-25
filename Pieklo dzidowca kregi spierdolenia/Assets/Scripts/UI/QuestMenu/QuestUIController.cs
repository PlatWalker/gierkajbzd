using System.Collections.Generic;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzd.Quests;
using jbzdy.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.UI.QuestMenu
{
    public class QuestUIController : UserInterfaceController
    {
        private const int BUTTON_HEIGHT = 40;

        [SerializeField]
        private GameObject questInListPrefab;
        [SerializeField]
        private RectTransform listTransform;
        [SerializeField]
        private RectTransform questDescription;
        [SerializeField]
        private TMP_Text questNameText;
        [SerializeField]
        private TMP_Text questDescriptionText;
        [SerializeField]
        private TMP_Text questGoldRewardText;
        [SerializeField]
        private TMP_Text questExpRewardText;
        [SerializeField]
        private TMP_Text questObjectiveText;
        [SerializeField]
        private RectTransform rewardsContent;
        [SerializeField]
        private RectTransform rewards;
        
        private List<Button> _questButtons;

        private Quest _currentQuest;
        private int _previousButtonIndex;

        private PlayerController _playerController;
        private QuestLog _questLog;
    
        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
            _questLog = _playerController.gameObject.GetComponent<QuestLog>();
        }
    
        private void Start()
        {
            _questButtons = new List<Button>();
            _questLog.Initialize();
            QuestLog.onQuestChange += UpdateQuests;
            UpdateQuests(new List<Quest>());
        }
        
        public override bool InitialActivationState() => false;
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnQuestLogOpened += OpenQuestLog;
            input.OnEscapeClick += () => gameObject.SetActive(false);
        }

        private void OpenQuestLog()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                _questLog.CheckQuestObjective();
                
                if(_currentQuest != null)
                    ShowTaskDetails(_currentQuest);
            }
        }

        #region Lewa część

        public void UpdateQuests(List<Quest> active)
        {
            HandleSizeChange(active.Count);
            UpdateQuestNames(active);

            if (!active.Contains(_currentQuest))
            {
                _currentQuest = null;
            }

            UpdateSelectedQuest();
            ShowQuestDetails(_currentQuest);
        }
        
        private void UpdateQuestText(Button questButton, Quest quest)
        {
            TMP_Text text = questButton.GetComponentInChildren<TMP_Text>();
            text.fontSize = 30;
            text.text = quest.title;
            text.color = quest.IsCompleted ? Color.green : Color.gray;
        }
        
        private void UpdateQuestNames(List<Quest> active)
        {
            for (int i = 0; i < active.Count; i++)
            {
                UpdateQuestText(_questButtons[i], active[i]);
            }

        }
        
        private void UpdateSelectedQuest()
        {
            if (_currentQuest == null) return;

            HighlightQuestButton(_questButtons[_previousButtonIndex], false);

            for(int i=0; i < _questButtons.Count; i++)
            {
                if(_questButtons[i].GetComponentInChildren<TMP_Text>().text == _currentQuest.title)
                {
                    HighlightQuestButton(_questButtons[i], true);
                    _previousButtonIndex = i;
                    return;
                }
            }
        }
        
        private void HandleSizeChange(int newCount)     
        {
            listTransform.sizeDelta = new Vector2(0, newCount * BUTTON_HEIGHT);
            int oldCount = _questButtons.Count;

            if (oldCount < newCount) {
            
                for (int i = oldCount; i < newCount; i++)
                {
                    _questButtons.Add(InitializeButton(i));
                
                }

            }
            else if(oldCount > newCount)
            {
                Destroy(_questButtons[_questButtons.Count - 1].gameObject);
                _questButtons.RemoveAt(_questButtons.Count - 1);

            }
        
        }

        private Button InitializeButton(int index)
        {
            Button button = Instantiate(questInListPrefab, listTransform).GetComponent<Button>();
            button.image.rectTransform.sizeDelta = new Vector2(0, BUTTON_HEIGHT);
            button.image.rectTransform.anchoredPosition = new Vector2(0, -BUTTON_HEIGHT * index);
            button.onClick.AddListener(delegate { QuestPress(button); });
            return button;
        }

        private void QuestPress(Button questButton)
        {
            HighlightQuestButton(_questButtons[_previousButtonIndex], false);
            HighlightQuestButton(questButton, true);
            _previousButtonIndex = _questButtons.IndexOf(questButton);
            _currentQuest = _questLog.GetQuestNo(_previousButtonIndex);
            ShowQuestDetails(_currentQuest);
        }

        private void HighlightQuestButton(Button questButton, bool active)
        {
            questButton.image.color = active ? Color.yellow : new Color(0, 0, 0, 0);
        }
        
        #endregion

        #region Prawa czesc

        private void ShowQuestDetails(Quest quest)
        {
            questDescription.gameObject.SetActive(quest != null);

            if (quest == null) return;

            questNameText.text = quest.title;
            questDescriptionText.text = quest.description;
            questGoldRewardText.text = quest.goldReward.ToString() + " golda";
            questExpRewardText.text = quest.expReward.ToString() + " expa";

            ShowTaskDetails(quest);


            questDescriptionText.rectTransform.sizeDelta = new Vector2(0, -questNameText.preferredHeight - 50); //gora
            rewardsContent.anchoredPosition = new Vector2(0, -(questDescriptionText.preferredHeight) * 0.66f); //dol
            rewards.anchoredPosition = new Vector2(0, - quest.tasks[quest.CurrentTask].goals.Count * 10);
            questDescription.sizeDelta = new Vector2(0,  questDescriptionText.preferredHeight - rewardsContent.rect.y + questNameText.preferredHeight);

        }

        private void ShowTaskDetails(Quest quest)
        {
            questObjectiveText.text = "";

            foreach (QuestGoal goal in quest.tasks[quest.CurrentTask].goals)
            {
                questObjectiveText.text += goal.ToString();
            }

        }

        #endregion
    }
}
