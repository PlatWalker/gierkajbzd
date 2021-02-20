using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using jbzdy.DialogueSystem.Enums;
using System.Collections.Generic;
using jbzdy.DialogueSystem.SO;

namespace jbzdy.DialogueSystem.Actions
{
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }

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

        public void SetText(string _name, string _textBox, StatCheckType statCheckType, int value)
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

        public void SetButtons(List<string> _texts, List<UnityAction> _unityActions)
        {
            buttons.ForEach(button => button.gameObject.SetActive(false));

            for (int i = 0; i < _texts.Count; i++)
            {
                buttonsTexts[i].text = _texts[i];
                buttons[i].gameObject.SetActive(true);
                buttons[i].onClick = new Button.ButtonClickedEvent();
                buttons[i].onClick.AddListener(_unityActions[i]);
            }
        }
    }
}



