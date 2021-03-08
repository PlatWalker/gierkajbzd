using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is key mapping
public enum Movement
{
    up = KeyCode.W,
    down = KeyCode.S,
    left = KeyCode.A,
    right = KeyCode.D
}

public enum Attack
{
    normal = KeyCode.Space
}

public class InputController : Singleton<InputController>
{
    // This is status of input, holds bool values for each input
    public struct MovementInputStatus
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
    }

    public struct AttackInputStatus
    {
        public bool normal;
    }

    // this is instance of above struct, unless we can make the strut a "singleton" this will do for now.
    public MovementInputStatus movementInputStatus;
    public AttackInputStatus attackInputStatus;
    public Vector3 mousePositionFlat;

    protected InputController() { }

    private void Update()
    {
        UpdateAttackInput();
        UpdateMovementInput();
        UpdateMousePosition();
    }

    private void UpdateAttackInput()
    {
        attackInputStatus.normal = WasPressed((KeyCode)Attack.normal);
    }

    void UpdateMovementInput()
    {
        movementInputStatus.up = Pressed((KeyCode)Movement.up);
        movementInputStatus.down = Pressed((KeyCode)Movement.down);
        movementInputStatus.left = Pressed((KeyCode)Movement.left);
        movementInputStatus.right = Pressed((KeyCode)Movement.right);
    }

    public bool WasPressed(KeyCode k)
    {
        return Input.GetKeyDown(k);
    }

    public bool Pressed(KeyCode k)
    {
        return Input.GetKey(k);
    }

    void UpdateMousePosition()
    {
        //mousePositionFlat = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f);
        mousePositionFlat = hitInfo.point;
        
    }
    

}
