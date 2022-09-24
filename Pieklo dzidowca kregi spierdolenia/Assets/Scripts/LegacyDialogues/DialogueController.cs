using System.Collections.Generic;
using jbzd.LegacyDialogues.NodesDatas;
using jbzd.MainHero;
using jbzdy.CharacterStats;
using jbzdy.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace jbzd.LegacyDialogues
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }

        [SerializeField] private PlayerStats playerStats = default;
        [SerializeField] private GameObject dialogueUI = default;
        [SerializeField] private Material transparentMaterial;
            
        [Header("NPC Texts")]
        [SerializeField] private TMP_Text NPCName = default;
        [SerializeField] private TMP_Text NPCAnswer = default;

        [Header("Talkers Images")]
        [SerializeField] private Image playerFaceImage = default;
        [SerializeField] private GameObject playerImageGO = default;
        [SerializeField] private Image NPCFaceImage = default;
        [SerializeField] private GameObject NPCImageGO = default;

        [Header("Answer Buttons")]
        [SerializeField] private Button answerButton1 = default;
        [SerializeField] private TMP_Text answerButtonText1 = default;
        [Space]
        [SerializeField] private Button answerButton2 = default;
        [SerializeField] private TMP_Text answerButtonText2 = default;
        [Space]
        [SerializeField] private Button answerButton3 = default;
        [SerializeField] private TMP_Text answerButtonText3 = default;
        [Space]
        [SerializeField] private Button answerButton4 = default;
        [SerializeField] private TMP_Text answerButtonText4 = default;

        public List<Button> answerButtons = new List<Button>();
        public List<TMP_Text> answerButtonsTexts = new List<TMP_Text>();
        private int statCheckNodeCount = 0;
        
        private PlayerController _playerController;
        
        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
        }
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
           
            ShowDialogueUI(false);
            AddButtonsToList();
        }

        private void Start()
        {
            NPCAnswer.enableAutoSizing = true;
            playerStats = _playerController.gameObject.GetComponent<PlayerStats>();
        }

        public void ShowDialogueUI(bool show)
        {
            dialogueUI.SetActive(show);
        }

        public void SetText(string newName, string newTextBox)
        {
            NPCName.text = newName;
            NPCAnswer.text = newTextBox;
        }

        public void SetImage(Sprite playerImage, Sprite npcImage)
        {
            playerImageGO.SetActive(true);
            NPCImageGO.SetActive(true);
            playerFaceImage.sprite = playerImage;
            NPCFaceImage.sprite = npcImage;

            if (playerImage == null) playerFaceImage.material = transparentMaterial;
        }

        //Setting up buttons to show them in dialogue options
        public void SetButtons(List<string> texts, List<UnityAction> unityActions, List<StatCheckNodeData> statCheckNodeDatas)
        {
            answerButtons.ForEach(button => button.gameObject.SetActive(false));
            UnityAction statCheck = null;

            statCheck = () =>
            {
                Debug.Log("Twoje statystyki są zbyt słabe, podszlifuj swoje umiejętności...");
            };
            
            
            if (statCheckNodeDatas.Count > 0)
            {
                for (int i = 0; i < statCheckNodeDatas.Count; i++)
                {
                    int playerValue = AddStatCheckPlayerValues(statCheckNodeDatas[i]);

                    answerButtonsTexts[i].text = "[" + statCheckNodeDatas[i].StatCheckType + " " + playerValue + "/" + statCheckNodeDatas[i].StatCheckValue + "] " + texts[i];
                    answerButtons[i].gameObject.SetActive(true);
                    answerButtons[i].onClick = new Button.ButtonClickedEvent();
                    answerButtons[i].onClick.AddListener(ValidateStatCheck(statCheckNodeDatas[i]) ? unityActions[i] : statCheck);
                }

                statCheckNodeCount = statCheckNodeDatas.Count;
            }   
            
            for (int i = statCheckNodeCount; i < texts.Count; i++)
            {
                answerButtonsTexts[i].text = texts[i];
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].onClick = new Button.ButtonClickedEvent();
                answerButtons[i].onClick.AddListener(unityActions[i]);
            }

            statCheckNodeCount = 0;
        }

        private bool ValidateStatCheck(StatCheckNodeData statCheckNodeData)
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
        
        private void AddButtonsToList()
        {
            answerButtons.Add(answerButton1);
            answerButtons.Add(answerButton2);
            answerButtons.Add(answerButton3);
            answerButtons.Add(answerButton4);

            answerButtonsTexts.Add(answerButtonText1);
            answerButtonsTexts.Add(answerButtonText2);
            answerButtonsTexts.Add(answerButtonText3);
            answerButtonsTexts.Add(answerButtonText4);
        }
    }
}



