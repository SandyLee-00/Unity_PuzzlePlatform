using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input System을 사용한 플레이어 입력 컨트롤러
/// 주석 부분은 PlayerMovementCharacterController를 사용한 코드 + 3인칭 시점을 위한 코드 (전체 플젝 7일 중 1일 사용, 하다가 못했다)
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnJumpEvent;
    public event Action OnInteractEvent;
    public event Action<float> OnShiftEvent;
    public event Action OnTabEvent;
    public event Action OnESCEvent;

/*    private Camera _camera;

    private PlayerInput _playerInput;

    [Header("Character Input Values")]
    public Vector2 look;
    public bool jump;
    public bool sprint;*/

/*    private void Start()
    {
        _camera = Camera.main;
    }*/

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

        /*LookInput(value.Get<Vector2>());*/
    }

/*    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }*/

    public void OnJump(InputValue value)
    {
        OnJumpEvent?.Invoke();
        /*JumpInput(value.isPressed);*/
    }

/*    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }*/

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
        /*SprintInput(value.isPressed);*/
    }
/*    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }*/

    /// <summary>
    /// Tab 키 눌러서 인벤토리 열기
    /// </summary>
    /// <param name="value"></param>
    public void OnPressTab(InputValue value)
    {
        OnTabEvent?.Invoke();
    }

    /// <summary>
    /// ESC 키 눌러서 커서 토글
    /// </summary>
    /// <param name="value"></param>
    public void OnPressESC(InputValue value)
    {
        OnESCEvent?.Invoke();
    }
}
