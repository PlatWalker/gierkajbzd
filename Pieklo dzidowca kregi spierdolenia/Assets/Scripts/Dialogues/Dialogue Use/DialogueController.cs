using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using jbzdy.CharacterStats;
using jbzdy.DialogueSystem.SO;
using jbzdy.DialogueSystem.Enums;
using System.Collections.Generic;

namespace jbzdy.DialogueSystem.Actions
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private GameObject dialogueUI;
        [Header("Text")]
        [SerializeField] private TMP_Text textName;
        [SerializeField] private TMP_Text textBox;
        [Header("Image")]
        [SerializeField] private Image leftImage;
        [SerializeField] private GameObject leftImageGO;
        [SerializeField] private Image rigthImage;
        [SerializeField] private GameObject rigthImageGO;
        [Header("Butttons")]
        [SerializeField] private Button button01;
        [SerializeField] private TMP_Text buttonText01;
        [Space]
        [SerializeField] private Button button02;
        [SerializeField] private TMP_Text buttonText02;
        [Space]
        [SerializeField] private Button button03;
        [SerializeField] private TMP_Text buttonText03;
        [Space]
        [SerializeField] private Button button04;
        [SerializeField] private TMP_Text buttonText04;

        private List<Button> buttons = new List<Button>();
        private List<TMP_Text> buttonsTexts = new List<TMP_Text>();

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

        public void ShowDialogueUI(bool _show)
        {
            dialogueUI.SetActive(_show);
        }

        public void SetText(string _name, string _textBox)
        {
            textName.text = _name;
            textBox.text = _textBox;
        }

        public void SetImage(Sprite _image, DialogueFaceImageType _dialogueFaceImageType)
        {
            leftImageGO.SetActive(false);
            rigthImageGO.SetActive(false);

            if (_image != null)
            {
                if (_dialogueFaceImageType == DialogueFaceImageType.Left)
                {
                    leftImage.sprite = _image;
                    leftImageGO.SetActive(true);
                }
                else
                {
                    rigthImage.sprite = _image;
                    rigthImageGO.SetActive(true);
                }
            }
        }

        public void SetButtons(List<string> _texts, List<UnityAction> _unityActions, List<StatCheckNodeData> statCheckNodeDatas)
        {
            buttons.ForEach(button => button.gameObject.SetActive(false));
            UnityAction statCheck = null;

            statCheck = () =>
            {
                Debug.Log("Chujowe staty");
            };

            if(statCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < statCheckNodeDatas.Count; i++)
                {
                    int playerValue = AddMatchingPlayerValues(statCheckNodeDatas[i]);

                    buttonsTexts[i].text = "[" + statCheckNodeDatas[i].statCheckType + " " + playerValue + "/" + statCheckNodeDatas[i].statCheckValue + "]" + _texts[i];
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].onClick = new Button.ButtonClickedEvent();

                    if(HasPassedCheck(statCheckNodeDatas[i]))
                    {
                        buttons[i].onClick.AddListener(_unityActions[i]);
                    }
                    else
                    {
                        buttons[i].onClick.AddListener(statCheck);
                    }
                }
            }

            for (int i = statCheckNodeDatas.Count; i < _texts.Count; i++)
            {
                buttonsTexts[i].text = _texts[i];
                buttons[i].gameObject.SetActive(true);
                buttons[i].onClick = new Button.ButtonClickedEvent();
                buttons[i].onClick.AddListener(_unityActions[i]);
            }
        }

        private int AddMatchingPlayerValues(StatCheckNodeData statCheckNodeData)
        {
            switch (statCheckNodeData.statCheckType)
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
                    return playerStats.Strenght.BaseValue;
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

        public bool HasPassedCheck(StatCheckNodeData statCheckNodeData)
        {
            switch (statCheckNodeData.statCheckType)
            {
                case StatCheckType.Exp:
                    if (statCheckNodeData.statCheckValue >= playerStats.ExperiencePoints)
                        return false;
                    else
                        return true;
                case StatCheckType.Level:
                    if (statCheckNodeData.statCheckValue >= playerStats.Level)
                        return false;
                    else
                        return true;
                case StatCheckType.Health:
                    if (statCheckNodeData.statCheckValue >= playerStats.Health.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Mana:
                    if (statCheckNodeData.statCheckValue >= playerStats.Mana.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Armor:
                    if (statCheckNodeData.statCheckValue >= playerStats.Armor.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Damage:
                    if (statCheckNodeData.statCheckValue >= playerStats.Damage.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Strenght:
                    if (statCheckNodeData.statCheckValue >= playerStats.Strenght.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Agility:
                    if (statCheckNodeData.statCheckValue >= playerStats.Agility.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Intelligence:
                    if (statCheckNodeData.statCheckValue >= playerStats.Intelligence.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Vitality:
                    if (statCheckNodeData.statCheckValue >= playerStats.Vitality.BaseValue)
                        return false;
                    else
                        return true;
                case StatCheckType.Luck:
                    if (statCheckNodeData.statCheckValue >= playerStats.Luck.BaseValue)
                        return false;
                    else
                        return true;
                default:
                    return false;
            }
        }

        public PlayerStats PlayerStats
        {
            get
            {
                return playerStats;
            }
        }
    }
}



