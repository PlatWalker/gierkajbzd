using System;
using System.Linq;
using jbzd.Common.InputSystem;
using jbzdy.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using jbzdy.DialogueSystem.NodeDatas;

/// <summary>
/// by SilverWalker
/// </summary>

public class GameManager : Singleton<GameManager>
{
    public InputController GameInputController { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public UIController UIControllerInstance { get; private set; }
    
    [field: SerializeField]
    public DialoguesCheckPointsSO dialoguesCheckPointsSO { get; private set; }

    private void Awake()
    {
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
