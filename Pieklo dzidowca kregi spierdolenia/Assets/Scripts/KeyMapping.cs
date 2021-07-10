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
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;

    [SerializeField]
    private KeyCode playerMoveUp;
    [SerializeField]
    private KeyCode playerMoveDown;
    [SerializeField]
    private KeyCode playerMoveLeft;
    [SerializeField]
    private KeyCode playerMoveRight;
    
    [SerializeField]
    private KeyCode playerBasicAttack;

    public void Awake()
    {
        playerMovement.up = playerMoveUp;
        playerMovement.down = playerMoveDown;
        playerMovement.left = playerMoveLeft;
        playerMovement.right = playerMoveRight;

        playerAttack.basic = playerBasicAttack;
    }
}
