using jbzd.LegacyDialogues.NodesDatas;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.Editor
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
            EditorApplication.wantsToQuit += SaveBeforeExitAndConfirm;
        }

        private bool SaveBeforeExitAndConfirm()
        {
            Save();
            return EditorUtility.DisplayDialog("Zamykanie",
                "Edytowany dialog został zapisany. Czy chcesz kontynuować zamykanie Unity?",
                "Tak", "Nie");
        }

        private void OnLostFocus()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            Save();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
            EditorApplication.wantsToQuit -= SaveBeforeExitAndConfirm;
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
        
        public void Save()
        {
            if (currentDialogueContainer != null)
            {
                saveAndLoad.Save(currentDialogueContainer);
            }
        }
        
        public void Load()
        {
            if (currentDialogueContainer != null)
            {
                nameOfDialougeContainer.text = "Name:   " + currentDialogueContainer.name;
                saveAndLoad.Load(currentDialogueContainer);
            }
        }
    }
}
