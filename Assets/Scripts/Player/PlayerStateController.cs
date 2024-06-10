using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Default = 0,
    Idle = 1 << 0,
    Walk = 1 << 1,
    Jump = 1 << 2,
    GetHit = 1 << 3,
    Die = 1 << 4,
    Interact = 1 << 5,
    Run = 1 << 6,

    Jumpable = Walk | Idle | Run,
}

/// <summary>
/// Player의 상태를 관리하는 클래스
/// </summary>
public class PlayerStateController : MonoBehaviour
{
    /// <summary>
    /// State 바뀌면 InvokeStateChangeEvent로 애니메이션 변경해주기
    /// </summary>
    public PlayerState State
    {
        get { return _playerState; }
        set
        {
            if (_playerState != value)
            {
                _playerState = value;
                InvokeStateChangeEvent();
            }
        }
    }

    public event Action<PlayerState> OnStateChangeEvent;

    [SerializeField]
    private PlayerState _playerState;
    private PlayerState _previousState;

    private PlayerInputController _playerInputController;
    private PlayerMovement _playerMovement;
    private PlayerHeartStamina _playerHealthMana;
    private PlayerAttributeHandler _playerAttributeHandler;

    private void Awake()
    {
        _playerInputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerHealthMana = gameObject.GetOrAddComponent<PlayerHeartStamina>();
        _playerMovement = gameObject.GetOrAddComponent<PlayerMovement>();
        _playerAttributeHandler = gameObject.GetOrAddComponent<PlayerAttributeHandler>();

        _playerHealthMana.OnDamage += () => 
        {
            SoundManager.Instance.Play(Define.Sound.Effect, "knifeSlice");
            StartCoroutine(ResetStateAfterDelayCoroutine(PlayerState.GetHit, _playerAttributeHandler.CurrentAttribute.heartChangeDelay));
        };

        _playerHealthMana.OnDeath += () => 
        {
            Debug.Log("PlayerStateController::OnDeath()");
            SoundManager.Instance.Play(Define.Sound.Effect, "metalPot3");
            State = PlayerState.Die;
            StartCoroutine(WaitforDeadAnimation(1.5f));
        };

        _playerInputController.OnInteractEvent += () => 
        {
            SoundManager.Instance.Play(Define.Sound.Effect, "handleSmallLeather");
            StartCoroutine(ResetStateAfterDelayCoroutine(PlayerState.Interact)); 
        };

    }

    private void InvokeStateChangeEvent()
    {
        OnStateChangeEvent?.Invoke(_playerState);
    }

    private IEnumerator ResetStateAfterDelayCoroutine(PlayerState newState, float delay = 1f)
    {
        _previousState = _playerState;
        State = newState;
        PlayerState tempState = _playerState;
        _playerMovement.IsMoveable = false;

        yield return new WaitForSeconds(delay);
        if (_playerState == tempState) // 이 동안 상태가 변하지 않았는지 확인
        {
            State = _previousState;
        }
        _playerMovement.IsMoveable = true;
    }

    private IEnumerator WaitforDeadAnimation(float delay)
    {
        Debug.Log("PlayerStateController::WaitforDeadAnimation()");
        yield return new WaitForSeconds(delay);

        _playerMovement.ToggleCursor();
        GameManager.Instance.GameOver();
    }
}
