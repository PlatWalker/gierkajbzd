using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace jbzdy.DialogueSystem.NodeDatas
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
