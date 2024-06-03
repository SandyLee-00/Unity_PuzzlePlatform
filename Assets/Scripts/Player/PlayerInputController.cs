using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnJumpEvent;

    private Camera _camera;

    private PlayerInput _playerInput;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized;

        OnMoveEvent?.Invoke(moveInput);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void OnLook(InputValue value)
    {
        Vector2 mouseDelta = value.Get<Vector2>();

        Mathf.Clamp(mouseDelta.x, -1, 1);
        
        OnLookEvent?.Invoke(mouseDelta);
    }

    public void OnJump(InputValue value)
    {
        OnJumpEvent?.Invoke();
    }

}
