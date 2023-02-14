using System.Collections.Generic;
using jbzd.Common.InputSystem.Inputs;
using jbzd.Dialogues.RuntimeData;
using jbzd.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace jbzd.Dialogues
{
    public class DialogueUIController : UserInterfaceController
    {
        private DialogueManager _dialogueManager;
        
        
        [SerializeField]
        private TMP_Text dialogueText;
        [SerializeField]
        private TMP_Text npcName;
        [SerializeField]
        private Image npcImage;
        [SerializeField]
        private Image playerImage;

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField]
        private RectTransform listTransform;

        private List<Button> _choiceButtons = new();
        
        [Inject]
        public void Construct(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
            _dialogueManager.OnDialogueEnded += ShowDialogueUI;
            _dialogueManager.OnDialogueStarted += ShowDialogueUI;
        }

        private void ShowDialogueUI()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void ButtonClick(int index)
        {
            var nextNodeId = _dialogueManager.CurrentNode.Choices[index].NextDialogue;

            if (nextNodeId == null)
            {
                ShowDialogueUI();
                return;
            }
            
            _dialogueManager.RunNode(nextNodeId);
        }

        public void ShowText(string text)
        {
            dialogueText.text = text;
        }

        public void SetNpcImage(Sprite image)
        {
            npcImage.sprite = image;
        }

        public void SetPlayerImage(Sprite image)
        {
            playerImage.sprite = image;
        }
        
        public void SetNpcName(string npcName)
        {
            this.npcName.text = npcName;
        }
        
        public void SetChoices(List<ChoiceRuntimeData> choiceList)
        {
            foreach (var button in _choiceButtons)
            {
                Destroy(button.gameObject);
            }

            _choiceButtons.Clear();
            for (var i = 0; i<choiceList.Count ; i++)
            {
                _choiceButtons.Add(InitializeChoice(choiceList[i], i));
            }
        }

        private Button InitializeChoice(ChoiceRuntimeData choice, int index)
        {
            Button button = Instantiate(buttonPrefab, listTransform).GetComponent<Button>();
            button.image.rectTransform.sizeDelta = new Vector2(200, 40);
            button.image.rectTransform.anchoredPosition = new Vector2(-110, 40 * index);
            button.onClick.AddListener(delegate { ButtonClick(index); });
            
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.fontSize = 20;
            text.text = choice.Text;
            
            return button;
        }
        
        public override bool InitialActivationState() => false;
        public override void ConnectInputToHandler(UserInterfaceInput input)
        {
            input.OnEscapeClick += () => gameObject.SetActive(false);
        }

    }
}
