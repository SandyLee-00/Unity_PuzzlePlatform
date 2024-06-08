using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어 움직임 컴포넌트
/// Walk, Run, Jump, Idle 상태를 관리한다
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
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

    [Header("Run")]
    private bool isShift = false;

    private void Awake()
    {
        _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
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
            _rigidbody.AddForce(Vector3.up * _playerAttributeHandler.CurrentAttribute.jumpForce, ForceMode.Impulse);
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
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

        // 마우스 움직임 -> 플레이어 회전, 카메라 회전
        LookFixedUpdate(mouseDelta);
    }

    private void JumpFixedUpdate()
    {
        // 땅에서 떨어져있으면 Jump or Fall, 움직이지 못함
        if (jumpKeepingCount > 0)
        {
            jumpKeepingCount--;
            return;
        }

        // 점프 하고 땅에 닿으면 이전 상태로 변경
        if (IsGrounded())
        {
            _playerStateController.State = _previousState;
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
            _rigidbody.velocity = new Vector3(move.x, _rigidbody.velocity.y, move.z);
            _playerStateController.State = PlayerState.Run;
            return;
        }
        // Shift 누르지 않거나 Mp 소모 불가능하면 걷기
        else
        {
            Vector3 move = direction * _playerAttributeHandler.CurrentAttribute.moveSpeed;
            _rigidbody.velocity = new Vector3(move.x, _rigidbody.velocity.y, move.z);
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

        transform.rotation = Quaternion.Euler(0, yaw, 0);
        _camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    /// <summary>
    /// Jump 후에 GroundLayer에 닿았는지 확인
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        Debug.DrawRay(rays[0].origin, rays[0].direction * 0.5f, Color.magenta);
        Debug.DrawRay(rays[1].origin, rays[1].direction * 0.5f, Color.magenta);
        Debug.DrawRay(rays[2].origin, rays[2].direction * 0.5f, Color.magenta);
        Debug.DrawRay(rays[3].origin, rays[3].direction * 0.5f, Color.magenta);

        return false;
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
