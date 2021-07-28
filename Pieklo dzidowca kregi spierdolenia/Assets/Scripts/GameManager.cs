using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// by SilverWalker
/// </summary>

public class GameManager : Singleton<GameManager>
{
    public InputController GameInputController { get; private set; }
    public GameObject PlayerObject { get; private set; }
    public GameObject test;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        test = GameObject.FindGameObjectWithTag("Player");
        GameInputController = gameObject.AddComponent<InputController>();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }
}
