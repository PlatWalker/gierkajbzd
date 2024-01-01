using System.Collections.Generic;
using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.RuntimeData;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace jbzd.Dialogues.Editor
{
    public class DialogueEditorWindow : EditorWindow
    {
        public ContainerSO dialogueContainer;
        private DialogueGraphView _graphView;
        private Toolbar _warningbar;
        
        public List<string> warnings = new();
        private static int NumberOfWindowsOpened = 0;
        private int editorIndex = 0;
        

        
        [OnOpenAsset(1)]
        public static bool Open(int instanceId, int line)
        {
            Object item = EditorUtility.InstanceIDToObject(instanceId); 

            if (item is ContainerSO so)
            {
                CreateSOWindow(so);
                
            }
            else if (item is GraphSaveDataSO gso)
            {
                CreateSOWindow(IOUtility.AssetFromGuid<ContainerSO>(gso.ContainerID));
            }     
            
            return false;
           
            void CreateSOWindow(ContainerSO container)
            {
                NumberOfWindowsOpened++;
                var window = CreateInstance<DialogueEditorWindow>();
                window.titleContent = new GUIContent("Dialogue Graph");
                window.Show();
                window.dialogueContainer = container;
                IOUtility.Load(NumberOfWindowsOpened - 1);
            }
        }

        private void OnEnable()
        {
            editorIndex = NumberOfWindowsOpened-1;
            AddGraphView();
            AddToolbar();
            AddWarningbar();
            IOUtility.Initialize(_graphView, this);
            EditorApplication.wantsToQuit += SaveBeforeExitAndConfirm;
        }

        private void AddWarningbar()
        {
            _warningbar = new Toolbar();

            _warningbar.AddStyleSheets("WarningbarStyleSheet");

            rootVisualElement.Add(_warningbar);
        }

        private void OnDisable()
        {
            IOUtility.Delete(editorIndex);
            rootVisualElement.Remove(_graphView);
            EditorApplication.wantsToQuit -= SaveBeforeExitAndConfirm;
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            Button saveButton = DialogueElementUtility.CreateButton("Save", () => IOUtility.Save(editorIndex));
            Button validateButton = DialogueElementUtility.CreateButton("Validate",() => IOUtility.Validate(editorIndex));

            toolbar.Add(saveButton);
            toolbar.Add(validateButton);

            toolbar.AddStyleSheets("ToolbarStyleSheet");
            
            rootVisualElement.Add(toolbar);
        }

        private void AddGraphView()
        {
            _graphView = new DialogueGraphView(this);

            _graphView.StretchToParentSize();
            _graphView.Init();
            
            rootVisualElement.Add(_graphView);
        }
        
        private bool SaveBeforeExitAndConfirm()
        {
            IOUtility.Save(editorIndex);
            return EditorUtility.DisplayDialog("Zamykanie",
                "Edytowany dialog został zapisany. Czy chcesz kontynuować zamykanie Unity?",
                "Tak", "Nie");
        }

        public void ShowValidationResult(List<string> warningList)
        {
            _warningbar.Clear();
            
            if (warningList.Count == 0)
            {
                Label success = DialogueElementUtility.CreateReadOnlyText("Walidacja się powiodła");
                _warningbar.Add(success);

                return;
            }

            foreach (var warning in warningList)
            {
                Label text = DialogueElementUtility.CreateReadOnlyText(warning);
                
                _warningbar.Add(text);
            }
        }
    }
    
}