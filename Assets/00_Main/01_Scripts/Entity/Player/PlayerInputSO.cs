using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
{
    private Controls _controls;

    public Action OnShootKeyPressed;
    public Action OnJumpKeyPressed;
    public Action OnInteractKeyPressed;
    
    public Vector2 MoveDir { get; private set; }
    public Vector2 MousePos { get; private set; }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    { 
        if (context.performed)
            OnShootKeyPressed?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnInteractKeyPressed?.Invoke();
    }

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }
}
