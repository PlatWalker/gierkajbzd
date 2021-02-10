using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using jbzdy.DialogueSystem.DataContainers;

namespace jbzdy.DialogueSystem.managers
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueContainer dialogueToNarrate;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button choiceButtonPrefab;
        [SerializeField] private Transform buttonContainer;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void StartDialogue(DialogueContainer dialogue)
        {
            gameObject.SetActive(true);

            dialogueToNarrate = dialogue;
            NodeLinkData dialogueData = dialogueToNarrate.NodeLinks.First();

            NarrateDialogue(dialogueData.TargetNodeGuid);
        }

        private void NarrateDialogue(string nodeGUID)
        {
            var text = dialogueToNarrate.DialogueNodeDatas.Find(x => x.NodeGUID == nodeGUID).DialogueText;
            var answerChoices = dialogueToNarrate.NodeLinks.Where(x => x.BaseNodeGuid == nodeGUID);
            dialogueText.text = ProcessProperties(text);
            var buttons = buttonContainer.GetComponentsInChildren<Button>();

            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            foreach (var choice in answerChoices)
            {
                Button choiceButton = Instantiate(choiceButtonPrefab, buttonContainer);
                choiceButton.GetComponentInChildren<TMP_Text>().text = ProcessProperties(choice.PortName);
                choiceButton.onClick.AddListener(() => NarrateDialogue(choice.TargetNodeGuid));
            }
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogueToNarrate.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }
    }
}
