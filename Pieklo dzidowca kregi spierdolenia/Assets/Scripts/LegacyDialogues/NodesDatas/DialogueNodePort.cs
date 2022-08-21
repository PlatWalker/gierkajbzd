using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace jbzd.LegacyDialogues.NodesDatas
{
    [System.Serializable]
    public class DialogueNodePort
    {
        public string PortGuid;
        public string InputGuid;
        public string OutputGuid;
#if UNITY_EDITOR
        public Port MyPort;
#endif
        public TextField TextField;
        public string Text;
    }
}
