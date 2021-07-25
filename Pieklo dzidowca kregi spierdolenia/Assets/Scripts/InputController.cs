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

    public MovementInputStatus movementInputStatus;
    public AttackInputStatus attackInputStatus;
    public Vector3 mousePositionFlat;

    private void Update()
    {
        UpdateAttackInput();
        UpdateMovementInput();
        UpdateMousePosition();
    }

    private void UpdateAttackInput()
    {
        attackInputStatus.basic = WasPressed(playerAttack.basic);
    }

    private void UpdateMovementInput()
    {
        movementInputStatus.up = Pressed(playerMovement.up);
        movementInputStatus.down = Pressed(playerMovement.down);
        movementInputStatus.left = Pressed(playerMovement.left);
        movementInputStatus.right = Pressed(playerMovement.right);
    }

    private void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f);
        mousePositionFlat = hitInfo.point;
    }

    public bool WasPressed(KeyCode k)
    {
        return Input.GetKeyDown(k);
    }

    public bool Pressed(KeyCode k)
    {
        return Input.GetKey(k);
    }

}