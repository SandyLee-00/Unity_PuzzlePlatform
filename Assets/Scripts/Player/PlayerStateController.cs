using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Idle = 0,
    Move,
    Jump,
    Attack,
    Defend,
    GetHit,
    Die,
    Interact
}

/// <summary>
/// Player의 상태를 관리하는 클래스
/// </summary>
public class PlayerStateController : MonoBehaviour
{
    public PlayerState State { get { return _playerState; } set { _playerState = value; } }

    public event Action<PlayerState> OnStateChangeEvent;

    [SerializeField]
    private PlayerState _playerState;

    private PlayerInputController _playerInputController;

    private PlayerHealthMana _playerHealthMana;


    private void Awake()
    {
        _playerInputController = gameObject.GetOrAddComponent<PlayerInputController>();
        _playerHealthMana = gameObject.GetOrAddComponent<PlayerHealthMana>();

        _playerHealthMana.OnDamage += () => { _playerState = PlayerState.GetHit; };
        _playerHealthMana.OnDeath += () => { _playerState = PlayerState.Die; };

        _playerInputController.OnInteractEvent += () =>
        {
            if (_playerState == PlayerState.Idle)
            {
                _playerState = PlayerState.Interact;
                Debug.Log($"PlayerState: {_playerState}");
            }
        };
    }

    public void InvokeStateChangeEvent()
    {
        OnStateChangeEvent?.Invoke(_playerState);
    }
}
