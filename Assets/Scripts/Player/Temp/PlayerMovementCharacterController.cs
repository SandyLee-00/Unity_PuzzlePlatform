using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어 움직임 컴포넌트
/// Walk, Run, Jump, Idle 상태를 관리한다
/// </summary>
public class PlayerMovementCharacterController : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInputController _inputController;
    private PlayerAttributeHandler _playerAttributeHandler;
    private PlayerStateController _playerStateController;
    private PlayerHeartStamina _playerHealthMana;

    [Header("Move")]
    private Vector3 moveDirection;
    public bool IsMoveable { get; set; } = true;

    [Header("Look")]
    private Vector2 mouseDelta;
    private const float mouseSensitivity = 1;
    private float pitch = 0f;
    private float yaw = 0f;
    private float maxPitchAngle = 70f;
    Camera _camera;
    bool isLookable = true;

    [Header("Jump")]
    private LayerMask groundLayerMask;
    private PlayerState _previousState;
    private int jumpKeepingCount = 0;
    private float verticalVelocity = 0f;

    [Header("Run")]
    private bool isShift = false;

    private void Awake()
    {
        _characterController = gameObject.GetOrAddComponent<CharacterController>();
        _inputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerAttributeHandler = gameObject.GetOrAddComponent<PlayerAttributeHandler>();
        _playerStateController = gameObject.GetOrAddComponent<PlayerStateController>();
        _playerHealthMana = gameObject.GetOrAddComponent<PlayerHeartStamina>();
    }

    void Start()
    {
        _inputController.OnMoveEvent += Move;
        _inputController.OnJumpEvent += Jump;
        _inputController.OnLookEvent += Look;
        _inputController.OnShiftEvent += Run;
        _inputController.OnTabEvent += ToggleCursor;
        _inputController.OnESCEvent += ToggleCursor;

        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;

        groundLayerMask = LayerMask.GetMask(Define.GroundLayer);
    }

    private void Move(Vector2 moveInput)
    {
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
    }

    /// <summary>
    /// jumpForce만큼 점프한다
    /// Idle, Walk, Run 상태에서만 점프 가능
    /// </summary>
    private void Jump()
    {
        if (IsGrounded() && (_playerStateController.State & PlayerState.Jumpable) != 0)
        {
            verticalVelocity = _playerAttributeHandler.CurrentAttribute.jumpForce;
            _previousState = _playerStateController.State;
            _playerStateController.State = PlayerState.Jump;

            // FixedUpdate 0.02 * 5 = 0.1초 동안 점프 유지 : 점프 시작할 때 땅에 닿아있다고 판단해서 0.1초 동안 점프 유지
            jumpKeepingCount = 5;
        }
    }

    /// <summary>
    /// 점프 플랫폼에서 호출하는 점프 함수
    /// State Jump로 변경해준다 
    /// </summary>
    /// <param name="jumpForce"></param>
    public void JumpByOther(float jumpForce)
    {
        verticalVelocity = jumpForce;
        _playerStateController.State = PlayerState.Jump;

        // FixedUpdate 0.02 * 5 = 0.1초 동안 점프 유지 : 점프 시작할 때 땅에 닿아있다고 판단해서 0.1초 동안 점프 유지
        jumpKeepingCount = 5;
    }

    private void Look(Vector2 mouseDelta)
    {
        this.mouseDelta = mouseDelta;
    }

    private void Run(float shiftInput)
    {
        isShift = shiftInput > 0.1;
    }

    private void FixedUpdate()
    {
        if(_playerStateController.State == PlayerState.Jump)
        {
            JumpFixedUpdate();
        }

        // WASD -> 플레이어 이동, Shift -> 달리기
        if (IsGrounded() && jumpKeepingCount <= 0 && IsMoveable)
        {
            MoveFixedUpdate(moveDirection);
        }

        // Apply gravity
        if (!IsGrounded())
        {
            verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        _characterController.Move(gravityMove * Time.fixedDeltaTime);

        // 마우스 움직임 -> 플레이어 회전, 카메라 회전
        LookFixedUpdate(mouseDelta);
    }

    private void JumpFixedUpdate()
    {
        if (jumpKeepingCount > 0)
        {
            jumpKeepingCount--;
            return;
        }

        if (IsGrounded())
        {
            _playerStateController.State = _previousState;
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// 플레이어 움직임을 FixedUpdate에서 처리
    /// WASD -> 플레이어 이동, Shift -> 달리기
    /// </summary>
    /// <param name="moveDirection"></param>
    private void MoveFixedUpdate(Vector3 moveDirection)
    {
        // 움직임 입력이 없으면 Idle 상태로 변경
        if (moveDirection.magnitude < 0.1)
        {
            _playerStateController.State = PlayerState.Idle;
            return;
        }

        // 플레이어 이동 방향
        Vector3 direction = transform.forward * moveDirection.y + transform.right * moveDirection.x;

        // Shift 누르고 Mp 소모
        if (isShift && _playerHealthMana.ChangeStamina(-_playerAttributeHandler.CurrentAttribute.costStaminaRun))
        {
            Vector3 move = direction * _playerAttributeHandler.CurrentAttribute.moveSpeed * _playerAttributeHandler.CurrentAttribute.runMultiplier;
            _characterController.Move(move * Time.fixedDeltaTime);
            _playerStateController.State = PlayerState.Run;
            return;
        }
        // Shift 누르지 않거나 Mp 소모 불가능하면 걷기
        else
        {
            Vector3 move = direction * _playerAttributeHandler.CurrentAttribute.moveSpeed;
            _characterController.Move(move * Time.fixedDeltaTime);
            _playerStateController.State = PlayerState.Walk;
            return;
        }
    }

    /// <summary>
    /// 마우스 움직임을 FixedUpdate에서 처리
    /// </summary>
    /// <param name="mouseDelta"></param>
    private void LookFixedUpdate(Vector2 mouseDelta)
    {
        if (isLookable == false)
        {
            return;
        }

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        // transform.rotation = Quaternion.Euler(0, yaw, 0);
        // _camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    /// <summary>
    /// Jump 후에 GroundLayer에 닿았는지 확인
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        return _characterController.isGrounded;
    }

    /// <summary>
    /// UI 사용할 때 커서 보이게 하기, 플레이어 움직임 멈추기
    /// </summary>
    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        isLookable = !toggle;
    }

    public void PlayFootstepSound()
    {
        SoundManager.Instance.Play(Define.Sound.Effect, "footstep00");
    }
}
