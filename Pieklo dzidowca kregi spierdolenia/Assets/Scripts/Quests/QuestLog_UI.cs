using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLog_UI : MonoBehaviour
{
    private readonly int buttonHeight = 40;

    private QuestLog questLog;

    public GameObject questInListPrefab;
    public RectTransform listTransform;

    public RectTransform questDescription;
    public TMP_Text questNameText;
    public TMP_Text questDescriptionText;
    public TMP_Text questGoldRewardText;
    public TMP_Text questExpRewardText;
    public TMP_Text questObjectiveText;
    public RectTransform rewardsContent;

    private GameObject questLogObject;
    private List<Button> questButtons;

    private Quest currentQuest;
    private int previousButtonIndex;

    private void Start()    
    {
        questLog = GameManager.Instance.PlayerObject.GetComponent<QuestLog>();
        questLogObject = transform.GetChild(0).gameObject;
        questLogObject.SetActive(false);
        questButtons = new List<Button>();
        questLog.Initialize();
        QuestLog.onQuestChange += UpdateQuests;
        UpdateQuests(new List<Quest>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questLogObject.SetActive(!questLogObject.activeSelf);

            if (questLogObject.activeSelf)
            {
                questLog.CheckQuestObjective();
                
                if(currentQuest != null)
                    ShowTaskDetails(currentQuest);
            }
        }
    }

    //lewy
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
        text.text = quest.title;
        text.color = quest.isCompleted ? Color.green : Color.gray;
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
        listTransform.sizeDelta = new Vector2(0, newCount * buttonHeight);
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
        button.image.rectTransform.sizeDelta = new Vector2(0, buttonHeight);
        button.image.rectTransform.anchoredPosition = new Vector2(0, -buttonHeight * index);
        button.onClick.AddListener(delegate { QuestPress(button); });
        return button;
    }

    private void QuestPress(Button questButton)
    {
        HighlightQuestButton(questButtons[previousButtonIndex], false);
        HighlightQuestButton(questButton, true);
        previousButtonIndex = questButtons.IndexOf(questButton);
        currentQuest = questLog.getQuestNo(previousButtonIndex);
        ShowQuestDetails(currentQuest);
    }

    private void HighlightQuestButton(Button questButton, bool active)
    {
        questButton.image.color = active ? Color.yellow : new Color(0, 0, 0, 0);
    }

    //prawy

    private void ShowQuestDetails(Quest quest)
    {
        questDescription.gameObject.SetActive(quest != null);

        if (quest == null) return;

        questNameText.text = quest.title;
        questDescriptionText.text = quest.description;
        questGoldRewardText.text = quest.goldReward.ToString() + " golda";
        questExpRewardText.text = quest.expReward.ToString() + " expa";

        ShowTaskDetails(quest);

        questDescriptionText.rectTransform.sizeDelta = new Vector2(0, - questDescriptionText.preferredHeight - 10);
        rewardsContent.anchoredPosition = new Vector2(0, questDescriptionText.rectTransform.sizeDelta.y);
    }

    private void ShowTaskDetails(Quest quest)
    {
        questObjectiveText.text = "";

        foreach (QuestGoal goal in quest.tasks[quest.currentTask].goals)
        {
            questObjectiveText.text += goal.ToString();
        }
    }

}
