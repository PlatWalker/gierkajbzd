using System.Collections.Generic;
using System;
using jbzd.MainHero;
using jbzd.UI;
using jbzd.QuestSystem.QuestStructureElements;
using jbzd.Common.InputSystem.Inputs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace jbzd.QuestSystem
{
    public class QuestUIController : UserInterfaceController
    {



        private PlayerManager _playerManager;
        [SerializeField] private QuestManager _questManager;
        private List<Quest> _activeQuests;


        [SerializeField] private Slider _questListSlider;
        [SerializeField] private Slider _questDescriptionSlider;

        [SerializeField] private TMP_Text _questNameText;
        [SerializeField] private TMP_Text _questTasks;

        [SerializeField] private ScrollRect _questListScrollRect;
        [SerializeField] private ScrollRect _questDescriptionScrollRect;

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform _buttonContainer;

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public override bool InitialActivationState() => false;

        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnEscapeClick += OnEscapeClick;
            input.OnQuestLogOpened += OnQuestMenuOpened;
        }
        private void OnQuestMenuOpened()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            if(gameObject.activeSelf){
                _playerManager.CanPlayerMove = false;
                CreateQuestList();
                _questListSlider.onValueChanged.AddListener(OnQuestListSliderValueChanged);
                _questDescriptionSlider.onValueChanged.AddListener(OnQuestDescriptionSliderValueChanged);
            }
            else{
                _playerManager.CanPlayerMove = true;
            }
        }


        
        private void OnEscapeClick()
        {
            _playerManager.CanPlayerMove = true;
            gameObject.SetActive(false);
        }


        public void CreateQuestList()
        { 

            _activeQuests = _questManager.ActiveQuests;

            foreach (Transform child in _buttonContainer)
            {
                Destroy(child.gameObject);
            }
            if(_activeQuests.Count > 0){
                OnQuestButtonClicked(0);
                for (int i = 0; i < _activeQuests.Count; i++)
                {
                    Quest quest = _activeQuests[i];
                    GameObject buttonGO = Instantiate(buttonPrefab, _buttonContainer);
                    Button button = buttonGO.GetComponent<Button>();
                    button.GetComponentInChildren<TMP_Text>().text = quest.name;

                    int questIndex = i;
                    button.onClick.AddListener(() => OnQuestButtonClicked(questIndex));
                }
                
            }
            else{
                _questNameText.text = "Nie masz żadnych questów";
                _questTasks.text = "";
                DisableScrollRectMovement(_questDescriptionScrollRect);
                DisableSliderMovement(_questDescriptionSlider);
            }
            if(AreAllElementsVisible(_questListScrollRect)){
                DisableScrollRectMovement(_questListScrollRect);
                DisableSliderMovement(_questListSlider);
            }
            else{
                EnableScrollRectMovement(_questListScrollRect);
                EnableSliderMovement(_questListSlider);
            }
        }

        private void OnQuestButtonClicked(int questIndex)
        {
            _questDescriptionSlider.value = 1;
            _questDescriptionScrollRect.verticalNormalizedPosition = 1;
            Quest selectedQuest = _activeQuests[questIndex];
            _questNameText.text =  selectedQuest.name;
            _questTasks.text = "";
            foreach(GoalSO goal in selectedQuest.ActiveTask.Goals){
                if(goal is jbzd.QuestSystem.Goals.KillEnemiesGoal){
                    int index = selectedQuest.GoalName.IndexOf(goal.name);
                    _questTasks.text += goal.name + "   " + selectedQuest.GoalItemCollected[index]+"/"+selectedQuest.GoalCompletionItemCount[index] +"\n";

                }
                else{
                    _questTasks.text += goal.name+"\n";
                }
            }
            ResizeTextToFitContent();
            if(AreAllElementsVisible(_questDescriptionScrollRect)){
                DisableSliderMovement(_questDescriptionSlider);
                DisableScrollRectMovement(_questDescriptionScrollRect);
            }
            else{
                EnableScrollRectMovement(_questDescriptionScrollRect);
                EnableSliderMovement(_questDescriptionSlider);
            }

        }
        private void ResizeTextToFitContent()
        {
            _questTasks.ForceMeshUpdate();
            float preferredHeight = _questTasks.preferredHeight;
            RectTransform _questTasksrectTransform = _questTasks.GetComponent<RectTransform>();
            _questTasksrectTransform.sizeDelta = new Vector2(_questTasksrectTransform.sizeDelta.x, preferredHeight);
        }

        private bool AreAllElementsVisible(ScrollRect scrollRect)
        {
            RectTransform contentRect = scrollRect.content;
            RectTransform viewportRect = scrollRect.viewport;


            // Z jakiegos powodu musze updateować layout i to 3 razy, bo inaczej contentRect.rect.height jest zle przy pierwszym odpaleniu
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(viewportRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

            bool allElementsFullyVisible = contentRect.rect.height < viewportRect.rect.height;
            return allElementsFullyVisible;
        }

        private void OnQuestListSliderValueChanged(float value){
            _questListScrollRect.verticalNormalizedPosition = _questListSlider.value;
        }

        private void OnQuestDescriptionSliderValueChanged(float value){
            _questDescriptionScrollRect.verticalNormalizedPosition = _questDescriptionSlider.value;
        }

        private void DisableScrollRectMovement(ScrollRect scrollRect){
            scrollRect.vertical = false;
        }

        private void EnableScrollRectMovement(ScrollRect scrollRect){
            scrollRect.vertical = true;
        }

        private void DisableSliderMovement(Slider slider){
            slider.value = 1;
            slider.interactable = false;
        }

        private void EnableSliderMovement(Slider slider){
            slider.value = 1;
            slider.interactable = true;
        }
    }
}
