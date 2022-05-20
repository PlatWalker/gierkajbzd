using System;
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
    [field: SerializeField]
    public DialoguesCheckPointsSO dialoguesCheckPointsSO { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        GameInputController = gameObject.AddComponent<InputController>();
        dialoguesCheckPointsSO = Resources.FindObjectsOfTypeAll<DialoguesCheckPointsSO>()[0];
    }

    private void Start()
    {
        dialoguesCheckPointsSO = Instantiate(dialoguesCheckPointsSO);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }
}
