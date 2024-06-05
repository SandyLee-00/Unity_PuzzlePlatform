using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input System을 사용한 플레이어 입력 컨트롤러
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnJumpEvent;
    public event Action OnInteractEvent;
    public event Action<float> OnShiftEvent;
    public event Action OnTabEvent;

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
    /// 마우스 움직임에 따라 플레이어 좌우 회전, 카메라 상하좌우 회전
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

    /// <summary>
    /// E 키 눌러서 오브젝트와 상호작용
    /// </summary>
    /// <param name="value"></param>
    public void OnPressE(InputValue value)
    {
        OnInteractEvent?.Invoke();
    }

    /// <summary>
    /// Shift 키 눌러서 달리기
    /// </summary>
    /// <param name="value"></param>
    public void OnHoldShift(InputValue value)
    {
        OnShiftEvent?.Invoke(value.Get<float>());
    }

    /// <summary>
    /// Tab 키 눌러서 인벤토리 열기
    /// </summary>
    /// <param name="value"></param>
    public void OnPressTab(InputValue value)
    {
        OnTabEvent?.Invoke();
    }

}
