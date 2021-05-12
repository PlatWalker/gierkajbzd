using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using jbzdy.CharacterStats;
using jbzdy.DialogueSystem.SO;
using jbzdy.DialogueSystem.Enums;
using System.Collections.Generic;
using jbzdy.Inventory;

// <summary>
// Napisane przez sharashino
// 
// Controller odpowiadający za wyświetlanie okna z dialogiem
// </summary>
namespace jbzdy.DialogueSystem.Actions
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }

        [SerializeField] private PlayerStats playerStats = default;
        [SerializeField] private GameObject dialogueUI = default;

        [Header("Text")]
        [SerializeField] private TMP_Text textName = default;
        [SerializeField] private TMP_Text textBox = default;

        [Header("Image")]
        [SerializeField] private Image leftImage = default;
        [SerializeField] private GameObject leftImageGO = default;
        [SerializeField] private Image rightImage = default;
        [SerializeField] private GameObject rightImageGO = default;

        [Header("Buttons")]
        [SerializeField] private Button button01 = default;
        [SerializeField] private TMP_Text buttonText01 = default;
        [Space]
        [SerializeField] private Button button02 = default;
        [SerializeField] private TMP_Text buttonText02 = default;
        [Space]
        [SerializeField] private Button button03 = default;
        [SerializeField] private TMP_Text buttonText03 = default;
        [Space]
        [SerializeField] private Button button04 = default;
        [SerializeField] private TMP_Text buttonText04 = default;

        private List<Button> buttons = new List<Button>();
        private List<TMP_Text> buttonsTexts = new List<TMP_Text>();
        private int itemCheckNodeCount = 0;
        private int statCheckNodeCount = 0;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            
            ShowDialogueUI(false);

            buttons.Add(button01);
            buttons.Add(button02);
            buttons.Add(button03);
            buttons.Add(button04);

            buttonsTexts.Add(buttonText01);
            buttonsTexts.Add(buttonText02);
            buttonsTexts.Add(buttonText03);
            buttonsTexts.Add(buttonText04);
        }

        public void ShowDialogueUI(bool show)
        {
            dialogueUI.SetActive(show);
        }

        public void SetText(string newName, string newTextBox)
        {
            textName.text = newName;
            textBox.text = newTextBox;
        }

        public void SetImage(Sprite image, DialogueFaceImageType dialogueFaceImageType)
        {
            leftImageGO.SetActive(false);
            rightImageGO.SetActive(false);

            if (dialogueFaceImageType == DialogueFaceImageType.Left)
            {
                leftImage.sprite = image;
                leftImageGO.SetActive(true);
            }
            else
            {
                rightImage.sprite = image;
                rightImageGO.SetActive(true);
            }
        }

        public void SetButtons(List<string> texts, List<UnityAction> unityActions, List<StatCheckNodeData> statCheckNodeDatas, List<ItemCheckNodeData> itemCheckNodeDatas)
        {
            buttons.ForEach(button => button.gameObject.SetActive(false));
            UnityAction statCheck = null;
            UnityAction itemCheck = null;

            statCheck = () =>
            {
                Debug.Log("Chujowe staty");
            };

            itemCheck = () =>
            {
                Debug.Log("Nie dostaniesz itemu");
            };
            
            if (itemCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < itemCheckNodeDatas.Count; i++)
                {
                    switch (itemCheckNodeDatas[i].ItemCheckType)
                    {
                        case ItemCheckNodeType.GetItem when itemCheckNodeDatas[i].ItemCheckValue > 1:
                            buttonsTexts[i].text = "[Otrzymaj " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GetItem:
                            buttonsTexts[i].text = "[Otrzymaj " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GiveItem when itemCheckNodeDatas[i].ItemCheckValue > 1:
                            buttonsTexts[i].text = "[Oddaj " + itemCheckNodeDatas[i].ItemCheckValue + " " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                        case ItemCheckNodeType.GiveItem:
                            buttonsTexts[i].text = "[Oddaj " + itemCheckNodeDatas[i].NodeItem.itemName + "] " + texts[i];
                            break;
                    }

                    buttons[i].gameObject.SetActive(true);
                    buttons[i].onClick = new Button.ButtonClickedEvent();
                    buttons[i].onClick.AddListener(itemCheck);
                }
                
                itemCheckNodeCount = itemCheckNodeDatas.Count;
            }

            if (statCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < statCheckNodeDatas.Count; i++)
                {
                    int playerValue = AddStatCheckPlayerValues(statCheckNodeDatas[i]);

                    buttonsTexts[i + itemCheckNodeCount].text = "[" + statCheckNodeDatas[i].StatCheckType + " " + playerValue + "/" + statCheckNodeDatas[i].StatCheckValue + "] " + texts[i + itemCheckNodeCount];
                    buttons[i + itemCheckNodeCount].gameObject.SetActive(true);
                    buttons[i + itemCheckNodeCount].onClick = new Button.ButtonClickedEvent();

                    if (HasPassedStatCheck(statCheckNodeDatas[i]))
                    {
                        buttons[i + itemCheckNodeCount].onClick.AddListener(unityActions[i]);
                    }
                    else
                    {
                        buttons[i + itemCheckNodeCount].onClick.AddListener(statCheck);
                    }
                }

                statCheckNodeCount = statCheckNodeDatas.Count;
            }


            for (int i = statCheckNodeDatas.Count + itemCheckNodeDatas.Count; i < texts.Count; i++)
            {
                buttonsTexts[i].text = texts[i];
                buttons[i].gameObject.SetActive(true);
                buttons[i].onClick = new Button.ButtonClickedEvent();
                buttons[i].onClick.AddListener(unityActions[i]);
            }
        }

        private int AddStatCheckPlayerValues(StatCheckNodeData statCheckNodeData)
        {
            switch (statCheckNodeData.StatCheckType)
            {
                case StatCheckType.Exp:
                    return playerStats.Level;
                case StatCheckType.Level:
                    return playerStats.Level;
                case StatCheckType.Health:
                    return playerStats.Health.BaseValue;
                case StatCheckType.Mana:
                    return playerStats.Mana.BaseValue;
                case StatCheckType.Armor:
                    return playerStats.Armor.BaseValue;
                case StatCheckType.Damage:
                    return playerStats.Damage.BaseValue;
                case StatCheckType.Strenght:
                    return playerStats.Strength.BaseValue;
                case StatCheckType.Agility:
                    return playerStats.Agility.BaseValue;
                case StatCheckType.Intelligence:
                    return playerStats.Intelligence.BaseValue;
                case StatCheckType.Vitality:
                    return playerStats.Vitality.BaseValue;
                case StatCheckType.Luck:
                    return playerStats.Luck.BaseValue;
                default:
                    return playerStats.Luck.BaseValue;
            }
        }

        public bool HasPassedItemCheck(ItemCheckNodeData itemCheckNodeData)
        {
            switch (itemCheckNodeData.ItemCheckType)
            {
                case ItemCheckNodeType.GetItem:
                    return true;
                case ItemCheckNodeType.GiveItem:
                    {
                        if (InventoryClass.Instance.CheckForItem(itemCheckNodeData.NodeItem, itemCheckNodeData.ItemCheckValue))
                        {
                            return false;
                        }
                        else
                            return false;
                    }
                default:
                    break;
            }

            return false;
        }

        private bool HasPassedStatCheck(StatCheckNodeData statCheckNodeData)
        {
            switch (statCheckNodeData.StatCheckType)
            {
                case StatCheckType.Exp:
                    return statCheckNodeData.StatCheckValue < playerStats.ExperiencePoints;
                case StatCheckType.Level:
                    return statCheckNodeData.StatCheckValue < playerStats.Level;
                case StatCheckType.Health:
                    return statCheckNodeData.StatCheckValue < playerStats.Health.BaseValue;
                case StatCheckType.Mana:
                    return statCheckNodeData.StatCheckValue < playerStats.Mana.BaseValue;
                case StatCheckType.Armor:
                    return statCheckNodeData.StatCheckValue < playerStats.Armor.BaseValue;
                case StatCheckType.Damage:
                    return statCheckNodeData.StatCheckValue < playerStats.Damage.BaseValue;
                case StatCheckType.Strenght:
                    return statCheckNodeData.StatCheckValue < playerStats.Strength.BaseValue;
                case StatCheckType.Agility:
                    return statCheckNodeData.StatCheckValue < playerStats.Agility.BaseValue;
                case StatCheckType.Intelligence:
                    return statCheckNodeData.StatCheckValue < playerStats.Intelligence.BaseValue;
                case StatCheckType.Vitality:
                    return statCheckNodeData.StatCheckValue < playerStats.Vitality.BaseValue;
                case StatCheckType.Luck:
                    return statCheckNodeData.StatCheckValue < playerStats.Luck.BaseValue;
                default:
                    return false;
            }
        }
    }
}



