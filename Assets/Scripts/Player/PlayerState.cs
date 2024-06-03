using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle = 0,
    Move,
    Jump,
    Attack,
    Defend,
    GetHit,
    Die
}


public class PlayerState : MonoBehaviour
{
    public PlayerStateEnum State { get { return playerState; } set { playerState = value; } }

    public event Action<PlayerStateEnum> OnStateChangeEvent;

    [SerializeField]
    private PlayerStateEnum playerState;

    private PlayerInputController playerInputController;

    private PlayerStatusSystem PlayerHealthSystem;


    private void Awake()
    {
        playerInputController = gameObject.GetOrAddComponent<PlayerInputController>();
        PlayerHealthSystem = gameObject.GetOrAddComponent<PlayerStatusSystem>();

        PlayerHealthSystem.OnDamage += () => { playerState = PlayerStateEnum.GetHit; };
        PlayerHealthSystem.OnDeath += () => { playerState = PlayerStateEnum.Die; };
    }

    public void InvokeStateChangeEvent()
    {
        OnStateChangeEvent?.Invoke(playerState);
    }
}
