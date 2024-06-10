using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Player의 State에 따라 애니메이션을 변경하는 클래스
/// 애니메이션 관련 이벤트 받아서 처리 : PlayFootstepSound
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    Animator _animator;
    PlayerStateController _playerStateController;

    private void Awake()
    {
        _playerStateController = gameObject.GetOrAddComponent<PlayerStateController>();
        _playerStateController.OnStateChangeEvent += ChangeAnimation;

        _animator = GetComponent<Animator>();
    }

    private void ChangeAnimation(PlayerState playerStateEnum)
    {
        string State = playerStateEnum.ToString();
        _animator.CrossFade(State, 0.2f);
    }

    public void PlayFootstepSound()
    {
        SoundManager.Instance.Play(Define.Sound.Effect, "footstep00");
    }
}
