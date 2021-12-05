using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// By SilverWalker
/// </summary>

public class InputController : KeyMapping
{
    public struct MovementInputStatus
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
    }

    public struct AttackInputStatus
    {
        public bool basic;
    }

    public struct UIinputStatus
    {
        public bool inGameMenu;
    }

    public UIinputStatus uIinputStatus;
    public MovementInputStatus movementInputStatus;
    public AttackInputStatus attackInputStatus;
    public Vector3 mousePositionFlat;

    private LayerMask layerMask;
    
    private void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        UpdateAttackInput();
        UpdateMovementInput();
        UpdateMousePosition();

        UpdateUIInput();
    }

    private void UpdateUIInput()
    {
        uIinputStatus.inGameMenu = WasPressed(UIinput.inGameMenu);
    }

    private void UpdateAttackInput()
    {
        attackInputStatus.basic = WasPressed(PlayerAttack.basic);
    }

    private void UpdateMovementInput()
    {
        movementInputStatus.up = Pressed(PlayerMovement.up);
        movementInputStatus.down = Pressed(PlayerMovement.down);
        movementInputStatus.left = Pressed(PlayerMovement.left);
        movementInputStatus.right = Pressed(PlayerMovement.right);
    }

    private void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask);
        
        mousePositionFlat.x = hitInfo.point.x;
        mousePositionFlat.y = 0;
        mousePositionFlat.z = hitInfo.point.z;
    }

    private bool WasPressed(KeyCode k)
    {
        return Input.GetKeyDown(k);
    }

    private bool Pressed(KeyCode k)
    {
        return Input.GetKey(k);
    }

}