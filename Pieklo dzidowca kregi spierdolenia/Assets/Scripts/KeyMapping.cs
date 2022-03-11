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

public struct UIinputKey
{
    public KeyCode inGameMenu;
}

public class KeyMapping : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private UIinputKey uIinput;

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

    public PlayerMovement PlayerMovement { get => playerMovement; private set => playerMovement = value; }
    public PlayerAttack PlayerAttack { get => playerAttack; private set => playerAttack = value; }
    public UIinputKey UIinput { get => uIinput; private set => uIinput = value; }

    public void Awake()
    {
        //TODO implement observer pattern for detecting key change
        playerMovement.up = playerMoveUp;
        playerMovement.down = playerMoveDown;
        playerMovement.left = playerMoveLeft;
        playerMovement.right = playerMoveRight;

        playerAttack.basic = playerBasicAttack;

        uIinput.inGameMenu = inGameMenu;
    }
}
