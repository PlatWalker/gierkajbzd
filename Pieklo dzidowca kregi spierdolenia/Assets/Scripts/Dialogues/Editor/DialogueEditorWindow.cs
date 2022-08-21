using jbzd.Dialogues.Editor.Save;
using jbzd.Dialogues.Editor.Utilities;
using jbzd.Dialogues.ScriptableObjects;
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
        
       [OnOpenAsset(1)]
       public static bool Open(int instanceId, int line)
       {
           Object item = EditorUtility.InstanceIDToObject(instanceId);
           
           if (item is ContainerSO so)
           {
               var window = GetWindow<DialogueEditorWindow>("Dialogue Graph");
               window.dialogueContainer = so;
               IOUtility.Load();
           }
           else if (item is GraphSaveDataSO gso)
           {
               var window = GetWindow<DialogueEditorWindow>("Dialogue Graph");
               window.dialogueContainer = IOUtility.AssetFromGuid<ContainerSO>(gso.ContainerID);
               IOUtility.Load();
           }
           return false;
           
       }
       
       private void OnEnable()
       {
           AddGraphView();
           AddToolbar();
           IOUtility.Initialize(_graphView, this);
           EditorApplication.wantsToQuit += SaveBeforeExitAndConfirm;
       }
       
       private void OnDisable()
       {
           rootVisualElement.Remove(_graphView);
           EditorApplication.wantsToQuit -= SaveBeforeExitAndConfirm;
       }

       private void AddToolbar()
       {
           Toolbar toolbar = new Toolbar();

           Button saveButton = DialogueElementUtility.CreateButton("Save", IOUtility.Save);

           toolbar.Add(saveButton);

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
           IOUtility.Save();
           return EditorUtility.DisplayDialog("Zamykanie",
               "Edytowany dialog został zapisany. Czy chcesz kontynuować zamykanie Unity?",
               "Tak", "Nie");
       }
       
    }
    
}
