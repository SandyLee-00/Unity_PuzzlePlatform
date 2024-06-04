using System.Collections;
using UnityEngine;

/// <summary>
/// Player 현재 HP, MP 를 제외한 모든 스탯을 관리한다.
/// </summary>
public class PlayerAttributeHandler : MonoBehaviour
{
    [SerializeField]
    public PlayerAttributes CurrentAttribute { get; private set; }

    private void Awake()
    {
        CurrentAttribute = new PlayerAttributes();
    }

    // TODO: OnBuff 메서드 하나로 수정
    public void OnSpeedBuff(BuffItemData buffItemData)
    {
        StartCoroutine(ApplySpeedBuff(buffItemData));
    }

    private IEnumerator ApplySpeedBuff(BuffItemData buffItemData)
    {
        float baseMoveSpeed = CurrentAttribute.moveSpeed;
        CurrentAttribute.moveSpeed *= buffItemData.multiplierValue;

        yield return new WaitForSeconds(buffItemData.duration);

        CurrentAttribute.moveSpeed = baseMoveSpeed;
    }

    public void OnJumpBuff(BuffItemData buffItemData)
    {
        StartCoroutine(ApplyJumpBuff(buffItemData));
    }

    private IEnumerator ApplyJumpBuff(BuffItemData buffItemData)
    {
        float baseJumpForce = CurrentAttribute.jumpForce;
        CurrentAttribute.jumpForce *= buffItemData.multiplierValue;

        yield return new WaitForSeconds(buffItemData.duration);

        CurrentAttribute.jumpForce = baseJumpForce;
    }
}
