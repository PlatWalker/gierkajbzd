using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.Extensions;
using jbzd.Dialogues;
using jbzd.Dialogues.RuntimeData;
using jbzd.MainHero;
using jbzd.MinorSystems.InputSystem;
using jbzd.MinorSystems.InputSystem.Inputs;
using jbzd.UI.Hud;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.UI.Dialogues
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
        private GameObject npcImageHolder;
        [SerializeField]
        private GameObject playerImageHolder;
        [SerializeField]
        private Image imagePrefab;

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
        private float _answerOffset = 0;

        #endregion

        [Inject]
        public void Construct(DialogueManager dialogueManager, PlayerManager playerManager, UserInterfaceManager uiManager, InputManager inputManager)
        {
            _uiManager = uiManager;
            _dialogueManager = dialogueManager;
            _playerManager = playerManager;

            inputManager.GetInput<UserInterfaceInput>().OnSpaceClick += FinishSentenceEarly;
            _dialogueManager.OnDialogueEnded += CloseDialogueUI;
            _dialogueManager.OnDialogueStarted += OpenDialogueUI;

        }

        private void CloseDialogueUI()
        {
            for (var i = 0; i < scrollContent.childCount; i++)
            {
                Destroy(scrollContent.GetChild(i).gameObject);
            }
            ClearImages();
            _dialogueOffset = 0;
            _isTyping = false;

            if (_hudCanvas == null)
                _hudCanvas = _uiManager.GetUIController<HudUIController>();

            gameObject.SetActive(false);
            _hudCanvas.gameObject.SetActive(true);
            _playerManager.CanPlayerMove = true;
            Debug.Log($"Dialogue {_dialogueManager.CurrentDialogueContainer.name} unfreeze player");
        }

        private void OpenDialogueUI()
        {
            _playerManager.CanPlayerMove = false;
            Debug.Log($"Dialogue {_dialogueManager.CurrentDialogueContainer.name} freeze player");

            gameObject.SetActive(true);

            if (_hudCanvas == null)
                _hudCanvas = _uiManager.GetUIController<HudUIController>();

            _hudCanvas.gameObject.SetActive(false);
        }

        public void ShowInsideUI(string text, bool isPlayerTalking, List<ChoiceRuntimeData> choices, bool showPlayerResponse, Sprite npcSprite, Sprite playerSprite, List<Sprite> leftAdditionalImages, List<Sprite> rigthAdditionalImages)
        {
            ShowText(text, isPlayerTalking);
            SetChoices(choices, showPlayerResponse);

            ClearImages();
            SetNpcImage(npcSprite, rigthAdditionalImages);
            SetPlayerImage(playerSprite, leftAdditionalImages);
        }

        private void ButtonClick(int index, string text = "", bool showPlayerResponse = true)
        {
            var nextNodeId = _dialogueManager.CurrentNode.Choices[index].NextDialogue;

            if (nextNodeId == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if (showPlayerResponse)
                ShowText(text, true);

            _dialogueManager.RunNode(nextNodeId);
        }

        private void UpdatePreviousTexts()
        {
            var prev_texts = scrollContent.GetComponentsInChildren<TMP_Text>();

            foreach(var txt in prev_texts)
            {
                txt.color = new Color32(154, 154, 154, 255);
            }
        }

        private void ShowText(string text, bool isPlayer = false)
        {
            if(dialogSpriteBox.gameObject.activeSelf == false)
                dialogSpriteBox.gameObject.SetActive(true);

            dialogSpriteBox.gameObject.GetComponent<Image>().sprite = isPlayer ? playerSpriteBox : npcSpriteBox;
            UpdatePreviousTexts();
            TMP_Text textLine = Instantiate(textPrefab, scrollContent).GetComponent<TMP_Text>();
            textLine.rectTransform.anchoredPosition = new Vector2(0, _dialogueOffset);

            if (!isPlayer)
            {
                textLine.margin = new Vector4(145, textLine.margin.y, 0, textLine.margin.w);
            }

            textLine.fontSize = 15;
            textLine.alignment = TextAlignmentOptions.TopLeft;
            textLine.horizontalAlignment = HorizontalAlignmentOptions.Justified;
            textLine.color = Color.white;
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

        private Image CreateImage(Sprite sprite, GameObject parent, int offset = 0, bool darken = false)
        {
            Image img = Instantiate(imagePrefab, parent.transform);
            img.sprite = sprite;
            img.transform.position = new Vector3(img.transform.position.x + offset, img.transform.position.y, img.transform.position.z);
            if (darken)
            {
                img.color = new Color32(128, 128, 128, 255);
            }

            return img;
        }

        private void ClearImages()
        {
            var childNum = npcImageHolder.transform.childCount;
            for (var i = childNum-1; i >= 0; i--)
            {
                Destroy(npcImageHolder.transform.GetChild(i).gameObject);
            }

            childNum = playerImageHolder.transform.childCount;
            for (var i = childNum-1; i >= 0; i--)
            {
                Destroy(playerImageHolder.transform.GetChild(i).gameObject);
            }
        }

        private void SetNpcImage(Sprite image, List<Sprite> additionalImages)
        {
            if (image != null)
            {
                npcImageHolder.gameObject.SetActive(true);
                int offset = 0;
                foreach(var img in additionalImages)
                {
                    offset += 60;
                    CreateImage(img, npcImageHolder, offset, true).transform.parent = npcImageHolder.transform;
                }
                CreateImage(image, npcImageHolder).transform.SetParent(npcImageHolder.transform, false);
                return;
            }

            npcImageHolder.gameObject.SetActive(false);
        }

        private void SetPlayerImage(Sprite image, List<Sprite> additionalImages)
        {
            if (image != null)
            {
                playerImageHolder.gameObject.SetActive(true);
                int offset = 0;
                foreach (var img in additionalImages)
                {
                    offset -= 60;
                    CreateImage(img, playerImageHolder, offset, true).transform.parent = playerImageHolder.transform;
                }
                CreateImage(image, playerImageHolder).transform.SetParent(playerImageHolder.transform, false);
                return;
            }

            playerImageHolder.gameObject.SetActive(false);
        }

        private void PrepareChoices()
        {
            foreach (var button in _choiceButtons)
            {
                Destroy(button.gameObject);
            }
            _answerOffset = 0;
            _choiceButtons.Clear();
        }

        private void SetChoices(List<ChoiceRuntimeData> choiceList, bool showPlayerResponse = true)
        {
            PrepareChoices();

            for (var i = 0; i < choiceList.Count; i++)
            {
                int index = i;
                _choiceButtons.Add(InitializeButton(choiceList[index].Text, index, delegate { ButtonClick(index, choiceList[index].Text, showPlayerResponse); }));
            }
        }

        public Button InitializeButton(string buttonText, int index, UnityEngine.Events.UnityAction buttonAction)
        {
            Button button = Instantiate(buttonPrefab, listTransform).GetComponent<Button>();
            button.onClick.AddListener(buttonAction);
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.fontSize = 20;
            text.text = buttonText;
            float textHeight = text.preferredHeight;

            RectTransform buttonRect = button.GetComponent<RectTransform>();

            float preferredHeight = button.GetComponentInChildren<TMP_Text>().preferredHeight;
            buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, preferredHeight);

            if (index == 0)
            {
                button.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -textHeight / 2);
                _answerOffset = -textHeight;
            }
            else{
                _answerOffset -= textHeight/2;
                button.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _answerOffset);
                _answerOffset -= textHeight/2;
            }
            return button;
        }

        public void SetButtonChoices(List<string> choiceList, List<UnityEngine.Events.UnityAction> buttonsActions, bool withWypierdalaj = true)
        {
            PrepareChoices();
            var i = 0;
            for (i = 0; i < choiceList.Count; i++)
            {
                int index = i;
                _choiceButtons.Add(InitializeButton(choiceList[index], index, buttonsActions[index]));
            }

            if (!withWypierdalaj) return;

            int lastIndex = i+1;
            _choiceButtons.Add(InitializeButton("Wypierdalaj", lastIndex, delegate { _dialogueManager.EndDialogue(); }));
        }

        /// <summary>
        /// Gives player option of choosing what dialogue to play if there are multiple available. Otherwise it works like standard StartDialogue.
        /// </summary>
        /// <param name="dataList">List of tuples of ContainerSO which is dialogue to be played and string which is text that represents this dialogue. If string is empty name of the dialogue will be used.</param>
        /// <param name="initiateText">Text which is asked by NPC</param>
        /// <param name="npcSprite">Image on the rigth of the screen</param>
        /// <param name="playerSprite">Image on the left of the screen</param>
        public void StartDialogue(List<(ContainerSO, string)> dataList, string initiateText="", Sprite npcSprite = null, Sprite playerSprite=null)
        {
            if (dataList.Count == 1)
            {
                StartDialogue(dataList[0].Item1);
                return;
            }

            List<UnityEngine.Events.UnityAction> actions = new();

            foreach (var data in dataList)
            {
                actions.Add(delegate { _dialogueManager.StartDialogue(data.Item1); });
            }

            OpenDialogueUI();
            ShowText(initiateText, false);
            SetButtonChoices(dataList.Select(x => {
                if (string.IsNullOrEmpty(x.Item2)) return x.Item1.name.MakeReadableText();
                return x.Item2;
                }).ToList(), actions);
            SetNpcImage(npcSprite, new List<Sprite>());
            SetPlayerImage(playerSprite, new List<Sprite>());
        }

        public void StartDialogue(ContainerSO data)
        {
            _dialogueManager.StartDialogue(data);
            OpenDialogueUI();
        }

        public override bool InitialActivationState() => false;
    }
}
