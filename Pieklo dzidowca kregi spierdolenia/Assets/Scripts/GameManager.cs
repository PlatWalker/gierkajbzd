using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// by SilverWalker
/// </summary>

public class GameManager : Singleton<GameManager>
{
    public InputController GameInputController { get; private set; }
    public GameObject PlayerObject { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameInputController = gameObject.AddComponent<InputController>();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        
    }
}
