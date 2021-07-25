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

public class KeyMapping : MonoBehaviour
{
    protected PlayerMovement playerMovement;
    protected PlayerAttack playerAttack;

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

    public void Awake()
    {
        playerMovement.up = playerMoveUp;
        playerMovement.down = playerMoveDown;
        playerMovement.left = playerMoveLeft;
        playerMovement.right = playerMoveRight;

        playerAttack.basic = playerBasicAttack;
    }
}
