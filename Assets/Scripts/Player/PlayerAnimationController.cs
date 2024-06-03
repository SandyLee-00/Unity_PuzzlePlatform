using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    PlayerState playerState;

    private void Awake()
    {
        playerState = gameObject.GetOrAddComponent<PlayerState>();
        playerState.OnStateChangeEvent += ChangeAnimation;

    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void ChangeAnimation(PlayerStateEnum playerStateEnum)
    {
        switch (playerStateEnum)
        {
            case PlayerStateEnum.Idle:
                animator.CrossFade("Idle_Battle", 0.1f);
                break;
            case PlayerStateEnum.Move:
                animator.CrossFade("WalkForwardBattle", 0.1f);
                break;
            case PlayerStateEnum.Jump:
                animator.CrossFade("RunForwardBattle", 0.1f);
                break;
            case PlayerStateEnum.Attack:
                animator.CrossFade("Attack01", 0.1f);
                break;
            case PlayerStateEnum.Defend:
                animator.CrossFade("Defend", 0.1f);
                break;
            case PlayerStateEnum.GetHit:
                animator.CrossFade("GetHit", 0.1f);
                break;
            case PlayerStateEnum.Die:
                animator.CrossFade("Die", 0.1f);
                break;
        }
    }

}
