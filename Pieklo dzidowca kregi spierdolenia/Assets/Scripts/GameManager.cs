using UnityEngine;

/// <summary>
/// by SilverWalker
/// </summary>

public class GameManager : Singleton<GameManager>
{
    public InputController GameInputController { get; private set; }
    public GameObject PlayerObject { get; private set; }

    private void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        GameInputController = gameObject.AddComponent<InputController>();
    }
}
