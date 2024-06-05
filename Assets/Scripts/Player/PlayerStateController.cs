using System;
using System.Collections;
using System.Collections.Generic;
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
    Fall = 1 << 7,
    Climb = 1 << 8,

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

    private PlayerInputController _playerInputController;

    private PlayerHealthMana _playerHealthMana;

    private void Awake()
    {
        _playerInputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerHealthMana = gameObject.GetOrAddComponent<PlayerHealthMana>();

        _playerHealthMana.OnDamage += () => { State = PlayerState.GetHit; };
        _playerHealthMana.OnDeath += () => { State = PlayerState.Die; };

        _playerInputController.OnInteractEvent += () => { State = PlayerState.Interact; };
    }

    private void InvokeStateChangeEvent()
    {
        OnStateChangeEvent?.Invoke(_playerState);
    }
}
