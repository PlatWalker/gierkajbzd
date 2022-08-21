using System;
using System.Linq;
using jbzd.Common.InputSystem;
using jbzd.LegacyDialogues.NodesDatas;
using jbzd.Quests.QuestSpecificScripts.Level1;
using jbzdy.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// by SilverWalker
/// </summary>

public class GameManager : Singleton<GameManager>
{
    public InputController GameInputController { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public UIController UIControllerInstance { get; private set; }
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
        PlayerObject = GameObject.FindGameObjectWithTag("Player");

        GameInputController = gameObject.AddComponent<InputController>();
        dialoguesCheckPointsSO = Resources.Load<DialoguesCheckPointsSO>("DONT_RENAME_OR_MOVE_DialogueCheckpointsList");
        UIControllerInstance = FindObjectOfType<UIController>();
    }
    
    private void Start()
    {
        dialoguesCheckPointsSO = Instantiate(dialoguesCheckPointsSO);
    }
}
