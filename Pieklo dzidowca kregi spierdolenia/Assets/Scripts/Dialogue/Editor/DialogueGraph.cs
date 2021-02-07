using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;

    private string fileName = "New Narrative";

    [MenuItem("Graphs/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMinimap();
    }

    private void GenerateMinimap()
    {
        MiniMap miniMap = new MiniMap { anchored = true };
        miniMap.SetPosition(new Rect(10, 500, 200, 140));
        graphView.Add(miniMap);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        TextField fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid filename", "Please enter a valid file name", "OK");
        }

        GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(graphView);

        if(save)
        {
            saveUtility.SaveGraph(fileName);
        }
        else
        {
            saveUtility.LoadGraph(fileName);
        }
    }
}
