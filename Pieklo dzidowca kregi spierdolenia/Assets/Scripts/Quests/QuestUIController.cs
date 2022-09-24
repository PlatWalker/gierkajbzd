using System.Collections.Generic;
using jbzd.Common.InputSystem;
using jbzd.Common.InputSystem.Inputs;
using jbzd.MainHero;
using jbzd.UI.NewUI;
using jbzdy.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.Quests
{
    public class QuestUIController : UserInterfaceController
    {
        private const int BUTTON_HEIGHT = 40;

        private QuestLog _questLog;

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

        public GameObject QuestInListPrefab { get => questInListPrefab; set => questInListPrefab = value; }
        public RectTransform ListTransform { get => listTransform; set => listTransform = value; }

        public RectTransform QuestDescription { get => questDescription; set => questDescription = value; }
        public TMP_Text QuestNameText { get => questNameText; set => questNameText = value; }
        public TMP_Text QuestDescriptionText { get => questDescriptionText; set => questDescriptionText = value; }
        public TMP_Text QuestGoldRewardText { get => questGoldRewardText; set => questGoldRewardText = value; }
        public TMP_Text QuestExpRewardText { get => questExpRewardText; set => questExpRewardText = value; }
        public TMP_Text QuestObjectiveText { get => questObjectiveText; set => questObjectiveText = value; }
        public RectTransform RewardsContent { get => rewardsContent; set => rewardsContent = value; }
        public RectTransform Rewards { get => rewards; set => rewards = value; }

        private GameObject questLogObject;
        private List<Button> questButtons;

        private Quest currentQuest;
        private int previousButtonIndex;

        private PlayerController _playerController;
        private UserInterfaceInput _inputController;
        [Inject]
        public void Construct(
            PlayerController playerController,
            InputManager inputManager)
        {
            _inputController = inputManager.GetInput<UserInterfaceInput>();
            _playerController = playerController;
            _questLog = _playerController.gameObject.GetComponent<QuestLog>();
        }
    
        private void Start()    
        {
            questLogObject = transform.GetChild(0).gameObject;
            questLogObject.SetActive(false);
            questButtons = new List<Button>();
            _questLog.Initialize();
            QuestLog.onQuestChange += UpdateQuests;
            UpdateQuests(new List<Quest>());
            _inputController.OnQuestLogOpened += OpenQuestLog;
        }

        private void OpenQuestLog()
        {
            questLogObject.SetActive(!questLogObject.activeSelf);

            if (questLogObject.activeSelf)
            {
                _questLog.CheckQuestObjective();
                
                if(currentQuest != null)
                    ShowTaskDetails(currentQuest);
            }
        }
    
        //lewa część 
        public void UpdateQuests(List<Quest> active)
        {
            HandleSizeChange(active.Count);
            UpdateQuestNames(active);

            if (!active.Contains(currentQuest))
            {
                currentQuest = null;
            }

            UpdateSelectedQuest();
            ShowQuestDetails(currentQuest);
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
                UpdateQuestText(questButtons[i], active[i]);
            }

        }

        private void UpdateSelectedQuest()
        {
            if (currentQuest == null) return;

            HighlightQuestButton(questButtons[previousButtonIndex], false);

            for(int i=0; i < questButtons.Count; i++)
            {
                if(questButtons[i].GetComponentInChildren<TMP_Text>().text == currentQuest.title)
                {
                    HighlightQuestButton(questButtons[i], true);
                    previousButtonIndex = i;
                    return;
                }
            }
        }


        private void HandleSizeChange(int newCount)     
        {
            listTransform.sizeDelta = new Vector2(0, newCount * BUTTON_HEIGHT);
            int oldCount = questButtons.Count;

            if (oldCount < newCount) {
            
                for (int i = oldCount; i < newCount; i++)
                {
                    questButtons.Add(InitializeButton(i));
                
                }

            }
            else if(oldCount > newCount)
            {
                Destroy(questButtons[questButtons.Count - 1].gameObject);
                questButtons.RemoveAt(questButtons.Count - 1);

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
            HighlightQuestButton(questButtons[previousButtonIndex], false);
            HighlightQuestButton(questButton, true);
            previousButtonIndex = questButtons.IndexOf(questButton);
            currentQuest = _questLog.GetQuestNo(previousButtonIndex);
            ShowQuestDetails(currentQuest);
        }

        private void HighlightQuestButton(Button questButton, bool active)
        {
            questButton.image.color = active ? Color.yellow : new Color(0, 0, 0, 0);
        }

        //prawa część

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

    }
}
