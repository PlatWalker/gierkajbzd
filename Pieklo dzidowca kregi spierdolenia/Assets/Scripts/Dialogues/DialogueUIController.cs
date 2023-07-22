using System;
using System.Collections;
using System.Collections.Generic;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Dialogues.RuntimeData;
using jbzd.MainHero;
using jbzd.UI;
using jbzd.UI.Hud;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.Dialogues
{
    public class DialogueUIController : UserInterfaceController
    {
        #region Variables
        
        private DialogueManager _dialogueManager;
        private UserInterfaceManager _uiManager;
        private PlayerManager _playerManager;
        private HudUIController _hudCanvas = null;
        
        [Header("DialogueBox")]
        [SerializeField]
        private TMP_Text textPrefab;
        [SerializeField]
        private RectTransform scrollContent;
        [SerializeField]
        private RectTransform dialogSpriteBox;
        [SerializeField] 
        private Sprite playerSpriteBox;
        [SerializeField] 
        private Sprite npcSpriteBox;

        [Header("Images")]
        [SerializeField]
        private Image npcImage;
        [SerializeField]
        private Image playerImage;

        [Header("AnswerBox")]
        [SerializeField] 
        private GameObject buttonPrefab;
        [SerializeField]
        private RectTransform listTransform;

        [Header("Additional")] 
        [SerializeField]
        private float textSpeed = 0.1f;

        private Coroutine _typeDialogueCoroutine;
        private bool _isTyping;
        private TMP_Text _lastTextContainer;
        private string _lastText;
        
        private List<Button> _choiceButtons = new();
        private float _dialogueOffset = 0;
        
        #endregion
        
        [Inject]
        public void Construct(DialogueManager dialogueManager, PlayerManager playerManager, UserInterfaceManager uiManager)
        {
            _uiManager = uiManager;
            _dialogueManager = dialogueManager;
            _playerManager = playerManager;
            
            _dialogueManager.OnDialogueEnded += CloseDialogueUI;
            _dialogueManager.OnDialogueStarted += OpenDialogueUI;
            
        }

        private void CloseDialogueUI()
        {
            for(var i = 0; i < scrollContent.childCount; i++)
            {
                Destroy(scrollContent.GetChild(i).gameObject);   
            }
                
            _dialogueOffset = 0;
            _isTyping = false;
            
            if(_hudCanvas == null)
                _hudCanvas = _uiManager.GetUIController<HudUIController>();
            
            gameObject.SetActive(false);
            _playerManager.CanPlayerMove = true;
            Debug.Log($"Dialogue {_dialogueManager.CurrentDialogueContainer.name} unfreeze player");
        }
        
        private void OpenDialogueUI()
        {
            _playerManager.CanPlayerMove = false;
            Debug.Log($"Dialogue {_dialogueManager.CurrentDialogueContainer.name} freeze player");
            
            gameObject.SetActive(true);

            if(_hudCanvas == null)
                _hudCanvas = _uiManager.GetUIController<HudUIController>();
            
            _hudCanvas.gameObject.SetActive(false);
        }
        
        public void ShowInsideUI(string text, bool isPlayerTalking, List<ChoiceRuntimeData> choices, bool showPlayerResponse, Sprite npcSprite, Sprite playerSprite)
        {
            ShowText(text, isPlayerTalking);
            SetChoices(choices, showPlayerResponse);
            
            SetNpcImage(npcSprite);
            SetPlayerImage(playerSprite);        
        }
        
        private void ButtonClick(int index, string text = "", bool showPlayerResponse = true)
        {
            var nextNodeId = _dialogueManager.CurrentNode.Choices[index].NextDialogue;

            if (nextNodeId == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if(showPlayerResponse)
                ShowText(text, true);
            
            _dialogueManager.RunNode(nextNodeId);
        }

        private void ShowText(string text, bool isPlayer = false)
        {
            dialogSpriteBox.gameObject.GetComponent<Image>().sprite = isPlayer ? playerSpriteBox : npcSpriteBox;
            
            TMP_Text textLine = Instantiate(textPrefab, scrollContent).GetComponent<TMP_Text>();
            textLine.rectTransform.anchoredPosition = new Vector2(0, _dialogueOffset);
            
            if (!isPlayer)
            {
                textLine.margin = new Vector4(145, textLine.margin.y, 0, textLine.margin.w);
            }

            textLine.fontSize = 24;
            textLine.alignment = isPlayer ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.TopRight;
            textLine.color = isPlayer ? Color.green : Color.white;
            textLine.enableAutoSizing = false;

            if (_isTyping) FinishSentenceEarly();
            
            _typeDialogueCoroutine = StartCoroutine(TypeSentence(textLine, text));
            _dialogueOffset -= textLine.preferredHeight;
        }

        private IEnumerator TypeSentence(TMP_Text textLine, string text)
        {
            _isTyping = true;
            _lastTextContainer = textLine;
            _lastText = text;
            textLine.text = "";

            foreach (var letter in text.ToCharArray())
            {
                textLine.text += letter;
                textLine.rectTransform.sizeDelta =
                    new Vector2(textLine.rectTransform.sizeDelta.x, textLine.preferredHeight);
                yield return new WaitForSeconds(textSpeed);
            }
            
            _isTyping = false;
        }

        private void FinishSentenceEarly()
        {
            StopCoroutine(_typeDialogueCoroutine);
            _lastTextContainer.text = _lastText;
            _lastTextContainer.rectTransform.sizeDelta =
                new Vector2(_lastTextContainer.rectTransform.sizeDelta.x, _lastTextContainer.preferredHeight);
            _isTyping = false;
        }
        
        private void SetNpcImage(Sprite image)
        {
            if (image != null)
            {
                npcImage.gameObject.SetActive(true);
                npcImage.sprite = image;
                return;
            }
            
            npcImage.gameObject.SetActive(false);
        }

        private void SetPlayerImage(Sprite image)
        {
            if (image != null)
            {
                playerImage.gameObject.SetActive(true);
                playerImage.sprite = image;
                return;
            }
            
            playerImage.gameObject.SetActive(false);
        }
        
        private void SetChoices(List<ChoiceRuntimeData> choiceList, bool showPlayerResponse = true)
        {
            foreach (var button in _choiceButtons)
            {
                Destroy(button.gameObject);
            }

            _choiceButtons.Clear();
            for (var i = 0; i<choiceList.Count ; i++)
            {
                _choiceButtons.Add(InitializeChoice(choiceList[i], i, showPlayerResponse));
            }
        }

        private Button InitializeChoice(ChoiceRuntimeData choice, int index, bool showPlayerResponse = true)
        {
            Button button = Instantiate(buttonPrefab, listTransform).GetComponent<Button>();
            button.onClick.AddListener(delegate { ButtonClick(index, choice.Text, showPlayerResponse); });
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.fontSize = 20;
            text.text = choice.Text;

            var textHeight = text.preferredHeight;
            button.gameObject.GetComponent<RectTransform>().anchoredPosition = 
                new Vector2(0, (-textHeight / 2) - textHeight * index - (float)(textHeight * 0.1));
            
            return button;
        }
        
        public override bool InitialActivationState() => false;
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            
        }
    }
}
