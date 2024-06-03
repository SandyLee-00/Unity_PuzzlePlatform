using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerInputController inputController;
    private PlayerStatHandler playerStatHandler;
    private PlayerState playerState;
    private PlayerStatusSystem playerStatusSystem;

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
        inputController = gameObject.GetOrAddComponent<PlayerInputController>();
        playerStatHandler = gameObject.GetOrAddComponent<PlayerStatHandler>();
        playerState = gameObject.GetOrAddComponent<PlayerState>();
        playerStatusSystem = gameObject.GetOrAddComponent<PlayerStatusSystem>();
    }

    void Start()
    {
        inputController.OnMoveEvent += Move;
        inputController.OnJumpEvent += Jump;
        inputController.OnLookEvent += Look;

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
            playerState.State = PlayerStateEnum.Move;
            playerState.InvokeStateChangeEvent();
        }
        else
        {
            playerState.State = PlayerStateEnum.Idle;
            playerState.InvokeStateChangeEvent();
        }
    }

    private void Jump()
    {
        if (playerState.State == PlayerStateEnum.Idle && playerStatusSystem.ChangeMP(costMPJump))
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerState.State = PlayerStateEnum.Jump;
            playerState.InvokeStateChangeEvent();
        }
    }

    public void JumpByOther(float jumpForce)
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Look(Vector2 mouseDelta)
    {
        this.mouseDelta = mouseDelta;
    }

    private void MoveFixedUpdate(Vector3 moveDirection)
    {
        Vector3 direction = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        Vector3 move = direction * playerStatHandler.CurrentStat.moveSpeed;

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
