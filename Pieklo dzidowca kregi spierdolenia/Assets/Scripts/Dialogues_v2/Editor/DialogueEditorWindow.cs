using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace jbzd.dialogues.editor
{
    //[CustomEditor(typeof(DialogueSO))]
    public class DialogueEditorWindow : EditorWindow
    {
        private readonly string _defaultFileName = "NazwaPliku"; 
        
       [MenuItem("Dialogues/New dialogue")] //na razie zeby mozna bylo latwo odpalic
       public static void Open()
       {
           GetWindow<DialogueEditorWindow>("Dialogue Graph");
       }
       
       private void OnEnable()
       {
           AddGraphView();
           AddToolbar();
       }

       private void AddToolbar()
       {
           Toolbar toolbar = new Toolbar();

           TextField fileNameTextField = DialogueElementUtility.CreateTextField(_defaultFileName, "Nazwa pliku:");

           Button saveButton = DialogueElementUtility.CreateButton("Save");
           
           toolbar.Add(fileNameTextField);
           toolbar.Add(saveButton);

           toolbar.AddStyleSheets("ToolbarStyleSheet");
           
           rootVisualElement.Add(toolbar);
       }

       private void AddGraphView()
       {
           DialogueGraphView graphView = new DialogueGraphView(this);
           
           graphView.StretchToParentSize();
           graphView.Init();
           
           rootVisualElement.Add(graphView);
       }
       
    }
    
}
