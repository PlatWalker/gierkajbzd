using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerMovement
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
}

public struct PlayerAttack
{
    public KeyCode basic;
}

public struct UIinput
{
    public KeyCode inGameMenu;
}

public class KeyMapping : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public UIinput uIinput;

    [SerializeField]
    private KeyCode playerMoveUp = KeyCode.W;
    [SerializeField]
    private KeyCode playerMoveDown = KeyCode.S;
    [SerializeField]
    private KeyCode playerMoveLeft = KeyCode.A;
    [SerializeField]
    private KeyCode playerMoveRight = KeyCode.D;
    
    [SerializeField]
    private KeyCode playerBasicAttack = KeyCode.Mouse0;

    [SerializeField]
    private KeyCode inGameMenu = KeyCode.Escape;

    public void Awake()
    {
        playerMovement.up = playerMoveUp;
        playerMovement.down = playerMoveDown;
        playerMovement.left = playerMoveLeft;
        playerMovement.right = playerMoveRight;

        playerAttack.basic = playerBasicAttack;

        uIinput.inGameMenu = inGameMenu;
    }
}
