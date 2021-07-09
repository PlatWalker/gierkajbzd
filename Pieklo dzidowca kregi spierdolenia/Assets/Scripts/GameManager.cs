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
    public GameObject playerObject;
    public InputController inputController;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //inputController = new InputController();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        
    }
}
