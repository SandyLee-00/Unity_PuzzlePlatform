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
    private PlayerHealthMana _playerHealthMana;

    private Vector3 moveDirection;

    private Vector2 mouseDelta;
    private const float mouseSensitivity = 1;
    private float pitch = 0f;
    private float yaw = 0f;
    public float maxPitchAngle = 20f;
    Camera _camera;
    bool isLookable = true;

    public LayerMask groundLayerMask;

    bool isShift = false;

    private void Awake()
    {
        _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
        _inputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerAttributeHandler = gameObject.GetOrAddComponent<PlayerAttributeHandler>();
        _playerStateController = gameObject.GetOrAddComponent<PlayerStateController>();
        _playerHealthMana = gameObject.GetOrAddComponent<PlayerHealthMana>();
    }

    void Start()
    {
        _inputController.OnMoveEvent += Move;
        _inputController.OnJumpEvent += Jump;
        _inputController.OnLookEvent += Look;
        _inputController.OnShiftEvent += Run;
        _inputController.OnTabEvent += ToggleCursor;

        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;

        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Move(Vector2 moveInput)
    {
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0);

        if (moveDirection.magnitude > 0.01)
        {
            _playerStateController.State = PlayerState.Walk;
        }
        else
        {
            _playerStateController.State = PlayerState.Idle;
        }
    }

    /// <summary>
    /// jumpForce만큼 점프한다
    /// </summary>
    private void Jump()
    {
        if (!IsGrounded())
        {
            return;
        }

        _rigidbody.AddForce(Vector3.up * _playerAttributeHandler.CurrentAttribute.jumpForce, ForceMode.Impulse);
        _playerStateController.State = PlayerState.Jump;
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
        MoveFixedUpdate(moveDirection);
        
        if (isLookable)
        {
            LookFixedUpdate(mouseDelta);
        }

        if (_playerStateController.State == PlayerState.Jump && IsGrounded())
        {
            _playerStateController.State = PlayerState.Idle;
        }

    }

    private void MoveFixedUpdate(Vector3 moveDirection)
    {
        Vector3 direction = transform.forward * moveDirection.y + transform.right * moveDirection.x;

        Vector3 move;
        if (isShift && _playerHealthMana.ChangeMP(_playerAttributeHandler.CurrentAttribute.costMPRun))
        {
            move = direction * _playerAttributeHandler.CurrentAttribute.runSpeed;
            _playerStateController.State = PlayerState.Run;
        }
        else
        {
            move = direction * _playerAttributeHandler.CurrentAttribute.moveSpeed;
        }

        _rigidbody.velocity = new Vector3(move.x, _rigidbody.velocity.y, move.z);
    }

    private void LookFixedUpdate(Vector2 mouseDelta)
    {
        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        _camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

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
                Debug.DrawRay(rays[i].origin, rays[i].direction * 0.1f, Color.red);
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        isLookable = !toggle;
    }
}
