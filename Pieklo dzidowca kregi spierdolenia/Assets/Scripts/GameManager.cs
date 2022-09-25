using jbzd.LegacyDialogues.NodesDatas;
using jbzd.Quests.QuestSpecificScripts.Level1;
using jbzd.UI;
using UnityEngine;

namespace jbzd
{
    public class GameManager : Singleton<GameManager>
    {
        public UIControllerLegacy UIControllerInstance { get; private set; }
        //TODO Na szybko robie, ale trzeba podmienic to bez odpowiedzialnosci na jakis bardziej ogolny questcontroller.
        //TODO Ale generalnie tak chyba to pwoinno dzialac.
        public BezOdpowiedzialnosci QuestController { get; private set; }
    
        [field: SerializeField]
        public DialoguesCheckPointsSO dialoguesCheckPointsSO { get; private set; }

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            QuestController = FindObjectOfType<BezOdpowiedzialnosci>();
        
            dialoguesCheckPointsSO = Resources.Load<DialoguesCheckPointsSO>("DONT_RENAME_OR_MOVE_DialogueCheckpointsList");
            UIControllerInstance = FindObjectOfType<UIControllerLegacy>();
        }
    
        private void Start()
        {
            dialoguesCheckPointsSO = Instantiate(dialoguesCheckPointsSO);
        }
    }
}
