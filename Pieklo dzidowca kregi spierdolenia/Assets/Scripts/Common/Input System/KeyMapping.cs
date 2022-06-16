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

public struct Interact
{
    public KeyCode InteractKey;
}

public class KeyMapping : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    private UIinputKey _uiInput;
    private Interact _interactKey;

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
    
    [SerializeField]
    private KeyCode interactKeyCode = KeyCode.E;
    
    public PlayerMovement PlayerMovement => _playerMovement;
    public PlayerAttack PlayerAttack => _playerAttack;
    public UIinputKey UIinput => _uiInput;
    public Interact Interact => _interactKey;


    public void Awake()
    {
        //TODO implement observer pattern for detecting key change
        _playerMovement.up = playerMoveUp;
        _playerMovement.down = playerMoveDown;
        _playerMovement.left = playerMoveLeft;
        _playerMovement.right = playerMoveRight;

        _playerAttack.basic = playerBasicAttack;

        _uiInput.inGameMenu = inGameMenu;

        _interactKey.InteractKey = interactKeyCode;
    }
}
