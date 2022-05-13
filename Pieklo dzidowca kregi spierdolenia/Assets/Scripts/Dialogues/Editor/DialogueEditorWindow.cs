using jbzdy.DialogueSystem.NodeDatas;
using jbzdy.DialogueSystem.SaveLoad;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace jbzdy.DialogueSystem.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueContainerSO currentDialogueContainer;
        private DialogueGraphView graphView;
        private DialogueSaveAndLoad saveAndLoad;

        private ToolbarMenu toolbarMenu;
        private Label nameOfDialougeContainer;

        [OnOpenAsset(1)]
        public static bool ShowWindow(int instanceId, int line)
        {
            Object item = EditorUtility.InstanceIDToObject(instanceId);

            if (item is DialogueContainerSO)
            {
                DialogueEditorWindow window = (DialogueEditorWindow)GetWindow(typeof(DialogueEditorWindow));
                window.titleContent = new GUIContent("Dialogue Editor");
                window.currentDialogueContainer = item as DialogueContainerSO;
                window.minSize = new Vector2(500, 250);
                window.Load();
            }
            return false;
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            Load();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }

        private void ConstructGraphView()
        {
            graphView = new DialogueGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);

            saveAndLoad = new DialogueSaveAndLoad(graphView,this);
        }

        private void GenerateToolbar()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("GraphViewStyleSheet");
            rootVisualElement.styleSheets.Add(styleSheet);

            Toolbar toolbar = new Toolbar();

            // Save button.
            Button saveBtn = new Button()
            {
                text = "Save"
            };
            saveBtn.clicked += () =>
            {
                Save();
            };
            toolbar.Add(saveBtn);

            // Load button.
            Button loadBtn = new Button()
            {
                text = "Load"
            };
            loadBtn.clicked += () =>
            {
                Load();
            };
            toolbar.Add(loadBtn);

            // Name of current DialigueContainer you have open.
            nameOfDialougeContainer = new Label("");
            toolbar.Add(nameOfDialougeContainer);
            nameOfDialougeContainer.AddToClassList("nameOfDialogueContainer");

            rootVisualElement.Add(toolbar);
        }
        
        private void Save()
        {
            if (currentDialogueContainer != null)
            {
                saveAndLoad.Save(currentDialogueContainer);
            }
        }
        
        private void Load()
        {
            if (currentDialogueContainer != null)
            {
                nameOfDialougeContainer.text = "Name:   " + currentDialogueContainer.name;
                saveAndLoad.Load(currentDialogueContainer);
            }
        }
    }
}
