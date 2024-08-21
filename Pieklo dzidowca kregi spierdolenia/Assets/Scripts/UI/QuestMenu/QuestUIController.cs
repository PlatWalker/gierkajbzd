using System;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.Extensions;
using jbzd.MainHero;
using jbzd.QuestSystem;
using jbzd.QuestSystem.QuestStructureElements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.UI.QuestMenu
{
    public class QuestUIController : UserInterfaceController
    {
        private readonly Color _not_read_color = Color.white;
        private readonly Color _completed_color = new Color32(197, 220, 99, 255);
        private readonly Color _read_color = new Color32(154, 154, 154, 255);

        #region Variables
        [SerializeField] private QuestManager _questManager;
        private List<Quest> _allQuests;

        [SerializeField] private TMP_Text _questNameText;

        [SerializeField] private TMP_Text _levelNumber;
        [SerializeField] private Button _levelBtnLeft;
        [SerializeField] private Button _levelBtnRight;

        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private Transform _buttonContainer;

        [SerializeField] private TMP_Text _textPrefab;
        [SerializeField] private Transform _textContainer;

        private List<Transform> _descTexts = new List<Transform>();
        private PlayerManager _playerManager;
        private int _currentLevel;
        #endregion

        public override bool InitialActivationState() => false;

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        private void Awake()
        {
            _levelBtnLeft.onClick.AddListener(() => CreateLevelButtons(_currentLevel - 1));
            _levelBtnRight.onClick.AddListener(() => CreateLevelButtons(_currentLevel + 1));
        }

        public void OnEnable()
        {
            if (!gameObject.activeSelf) return;
            _allQuests = _questManager.AllQuestsFromLoadedMaps.Where(quest => quest.IsCompleted == true).ToList();
            _allQuests.AddRange(_questManager.ActiveQuests);
            CreateLevelButtons(_playerManager.currentKrag);            
        }

        private void CreateLevelButtons(int level)
        {
            _levelNumber.text = level.ToString();

            _levelBtnLeft.interactable = _allQuests.Any(x => x.QuestData.Krag < level);
            _levelBtnRight.interactable = _allQuests.Any(x => x.QuestData.Krag > level);

            _currentLevel = level;
            CreateQuestList();
        }

        public void CreateQuestList()
        { 

            
            var _kragQuests = _allQuests.Where(quest => quest.QuestData.Krag == _currentLevel).ToList();

            foreach (Transform child in _buttonContainer)
            {
                if (child.GetComponent<Button>() != null)
                    Destroy(child.gameObject);
            }

            if(_kragQuests.Count > 0){
                OnQuestButtonClicked(0);
                for (int i = 0; i < _kragQuests.Count; i++)
                {
                    Quest quest = _kragQuests[i];
                    GameObject buttonGO = Instantiate(_buttonPrefab, _buttonContainer);
                    Button button = buttonGO.GetComponent<Button>();
                    var btnText = button.GetComponentInChildren<TMP_Text>();
                    btnText.text = quest.name.MakeReadableText();
                    btnText.color = quest.IsCompleted ? _completed_color : quest.newUpdate ? _not_read_color : _read_color;
                    btnText.fontSize = quest.QuestData.IsMain ? 28 : 24;    

                    RectTransform buttonRect = button.GetComponent<RectTransform>();
                    
                    float preferredHeight = button.GetComponentInChildren<TMP_Text>().preferredHeight;
                    buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, preferredHeight+10f);
                    int questIndex = i;
                    button.onClick.AddListener(() =>
                    {
                        UpdateButtons(button);                      
                        OnQuestButtonClicked(_allQuests.IndexOf(quest));
                    });

                    if(i == 0)
                        button.GetComponent<Image>().color = new Color32(255, 255, 255, 51);
                }
                
            }
        }

        private void UpdateButtons(Button btn)
        {            
            foreach (Transform child in _buttonContainer)
            {
                if (child.GetComponent<Button>() != null)
                    child.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            }
            btn.GetComponent<Image>().color = new Color32(255, 255, 255, 51);

            var btnText = btn.GetComponentInChildren<TMP_Text>();
            if (btnText.color == _not_read_color)
            {
                btnText.color = _read_color;
            }
        }

        private void OnQuestButtonClicked(int questIndex)
        {
            Quest selectedQuest = _allQuests[questIndex];
            _questNameText.text = selectedQuest.name.MakeReadableText();           
            PrepareDescription();
            
            foreach (TaskSO task in selectedQuest.FinishedTasks)
            {
                CreateDescriptionText(task.Description, false);

                foreach (GoalSO goal in task.Goals)
                {
                    var desc = goal.Description;
                    if (goal is GoalInvolvingCollectingSO) desc += ": " + selectedQuest.GoalItemCollected[questIndex] + "/" + selectedQuest.GoalCompletionItemCount[questIndex];
                    CreateDescriptionText(desc, false);
                }
            }

            if (selectedQuest.ActiveTask != null)
            {
                CreateDescriptionText(selectedQuest.ActiveTask.Description, selectedQuest.newUpdate);
                foreach (GoalSO goal in selectedQuest.ActiveTask.Goals)
                {
                    var desc = goal.Description;
                    if (goal is GoalInvolvingCollectingSO) desc += ": " + selectedQuest.GoalItemCollected[questIndex] + "/" + selectedQuest.GoalCompletionItemCount[questIndex];
                    CreateDescriptionText(desc, selectedQuest.newUpdate);
                }
            }

            selectedQuest.newUpdate = false;
        }

        private void PrepareDescription()
        {
            foreach (Transform child in _descTexts)
            {
                Destroy(child.gameObject);
            }
            _descTexts.Clear();
        }

        private void CreateDescriptionText(string text, bool isNew=true)
        {
            TMP_Text textLine = Instantiate(_textPrefab, _textContainer);
            textLine.text = text;
            textLine.color = isNew ? _not_read_color : _read_color;

            float preferredHeight = textLine.preferredHeight;
            RectTransform _questTasksrectTransform = textLine.GetComponent<RectTransform>();
            _questTasksrectTransform.sizeDelta = new Vector2(_questTasksrectTransform.sizeDelta.x, preferredHeight);

            _descTexts.Add(textLine.transform);
        }

    }
}
