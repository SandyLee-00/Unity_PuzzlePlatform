using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

/// <summary>
/// 플레이어 움직임 컴포넌트
/// Walk, Run, Jump, Idle 상태를 관리한다
/// </summary>
public class PlayerMovementCharacterController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerInputController _inputController;
    private PlayerAttributeHandler _playerAttributeHandler;
    private PlayerStateController _playerStateController;
    private PlayerHeartStamina _playerHealthMana;
    private CharacterController _characterController;

    #region Move
    private Vector2 moveInput;
    #endregion

    #region Look
    private Vector2 mouseDelta;
    private const float mouseSensitivity = 1;
    private float pitch = 0f;
    private float yaw = 0f;
    public float maxPitchAngle = 20f;
    Camera _camera;
    bool isLookable = true;
    #endregion

    #region Jump
    public LayerMask groundLayerMask;
    private PlayerState _previousState;
    #endregion

    #region Run
    bool isShift = false;
    #endregion

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    // player
    private float _verticalVelocity;


    private GameObject _mainCamera;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        _inputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerAttributeHandler = gameObject.GetOrAddComponent<PlayerAttributeHandler>();
        _playerStateController = gameObject.GetOrAddComponent<PlayerStateController>();
        _playerHealthMana = gameObject.GetOrAddComponent<PlayerHeartStamina>();
        _characterController = gameObject.GetOrAddComponent<CharacterController>();
    }

    void Start()
    {
        _inputController.OnMoveEvent += Move;
        _inputController.OnLookEvent += Look;
        _inputController.OnShiftEvent += Run;
        _inputController.OnTabEvent += ToggleCursor;

        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main;

        GroundLayers = LayerMask.GetMask(Define.GroundLayer);
    }

    private void Move(Vector2 moveInput)
    {
        this.moveInput = new Vector3(moveInput.x, moveInput.y, 0);
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
        /*// WASD -> 플레이어 이동, Shift -> 달리기
        MoveFixedUpdate(moveInput);

        // 마우스 움직임 -> 플레이어 회전, 카메라 회전
        LookFixedUpdate(mouseDelta);*/
    }

    /// <summary>
    /// 플레이어 움직임을 FixedUpdate에서 처리
    /// WASD -> 플레이어 이동, Shift -> 달리기
    /// </summary>
    /// <param name="moveDirection"></param>
    private void MoveFixedUpdate(Vector3 moveDirection)
    {
        Debug.Log($"{_playerStateController.State} 점프 상테에서 다른 상태로 안바꿔줘야한다~~~~~~~~~~~");

        // 움직임 입력이 없으면 Idle 상태로 변경
        if (moveDirection.magnitude < 0.1)
        {
            _playerStateController.State = PlayerState.Idle;
            return;
        }

        // 플레이어 이동 방향
        Vector3 direction = transform.forward * moveDirection.y + transform.right * moveDirection.x;

        // Shift 누르고 Mp 소모 가능하면 달리기
        if (isShift && _playerHealthMana.ChangeStamina(-_playerAttributeHandler.CurrentAttribute.costStaminaRun))
        {
            Vector3 move = direction * _playerAttributeHandler.CurrentAttribute.runMultiplier;
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

        //transform.localEulerAngles = new Vector3(0, yaw, 0);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        _camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
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

    private void Update()
    {
        UpdateJumpAndGravity();
        UpdateGroundedCheck();
        UpdateMove();
    }

    private void UpdateMove()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        // 스프린트 상태에 따라 이동 속도 결정
        float targetSpeed = 0;
        if (_inputController.sprint)
        {
            targetSpeed = _playerAttributeHandler.CurrentAttribute.moveSpeed * _playerAttributeHandler.CurrentAttribute.runMultiplier;
        }
        else
        {
            targetSpeed = _playerAttributeHandler.CurrentAttribute.moveSpeed;
        }

        // 이동 입력이 없으면 이동 속도 0으로 설정
        if (moveInput == Vector2.zero) targetSpeed = 0.0f;

        // 현재 수평 속도
        float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

        float _speed = targetSpeed;

        // 입력 방향을 정규화
        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        // 입력이 있으면 플레이어 회전
        float _targetRotation = 0.0f;
        if (moveInput != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;

            // 플레이어가 카메라를 기준으로 입력 방향을 바라보도록 회전
            transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // 플레이어 이동
        _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

    }

    private void UpdateJumpAndGravity()
    {
        // 이미 땅에서 떨어져 있으면 더 이상 처리하지 않음
        if (Grounded == false)
        {
            return;
        }


    }

    private void UpdateGroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {

    }
}
