using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어 움직임 컴포넌트
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

    private const float jumpForce = 5f;
    private const float costMPJump = -40f;

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

        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;
    }
    private void FixedUpdate()
    {
        MoveFixedUpdate(moveDirection);
        LookFixedUpdate(mouseDelta);
    }

    private void Move(Vector2 moveInput)
    {
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0);

        if (moveDirection.magnitude > 0)
        {
            _playerStateController.State = PlayerState.Walk;
            _playerStateController.InvokeStateChangeEvent();
        }
        else
        {
            _playerStateController.State = PlayerState.Idle;
            _playerStateController.InvokeStateChangeEvent();
        }
    }

    private void Jump()
    {
        if(_rigidbody.velocity.y > 0.1 || _rigidbody.velocity.y < -0.1)
        {
            return;
        }

        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _playerStateController.State = PlayerState.Jump;
        _playerStateController.InvokeStateChangeEvent();
    }

    public void JumpByOther(float jumpForce)
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _playerStateController.State = PlayerState.Jump;
        _playerStateController.InvokeStateChangeEvent();
    }

    private void Look(Vector2 mouseDelta)
    {
        this.mouseDelta = mouseDelta;
    }

    private void MoveFixedUpdate(Vector3 moveDirection)
    {
        Vector3 direction = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        Vector3 move = direction * _playerAttributeHandler.CurrentAttribute.moveSpeed;

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

}
