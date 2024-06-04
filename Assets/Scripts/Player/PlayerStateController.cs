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
    Die
}

/// <summary>
/// Player의 상태를 관리하는 클래스
/// </summary>
public class PlayerStateController : MonoBehaviour
{
    public PlayerState State { get { return playerState; } set { playerState = value; } }

    public event Action<PlayerState> OnStateChangeEvent;

    [SerializeField]
    private PlayerState playerState;

    private PlayerInputController playerInputController;

    private PlayerHealthMana PlayerHealthSystem;


    private void Awake()
    {
        playerInputController = gameObject.GetOrAddComponent<PlayerInputController>();
        PlayerHealthSystem = gameObject.GetOrAddComponent<PlayerHealthMana>();

        PlayerHealthSystem.OnDamage += () => { playerState = PlayerState.GetHit; };
        PlayerHealthSystem.OnDeath += () => { playerState = PlayerState.Die; };
    }

    public void InvokeStateChangeEvent()
    {
        OnStateChangeEvent?.Invoke(playerState);
    }
}
