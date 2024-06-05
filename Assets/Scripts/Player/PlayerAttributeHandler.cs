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

    public void OnBuff(BuffItemData buffItemData)
    {
        switch (buffItemData.type)
        {
            case BuffItemType.Speed:
                StartCoroutine(ApplySpeedBuff(buffItemData));
                break;

            case BuffItemType.Jump:
                StartCoroutine(ApplyJumpBuff(buffItemData));
                break;
        }
    }

    private IEnumerator ApplySpeedBuff(BuffItemData buffItemData)
    {
        float baseMoveSpeed = CurrentAttribute.moveSpeed;
        CurrentAttribute.moveSpeed *= buffItemData.multiplierValue;

        yield return new WaitForSeconds(buffItemData.duration);

        CurrentAttribute.moveSpeed = baseMoveSpeed;
    }

    private IEnumerator ApplyJumpBuff(BuffItemData buffItemData)
    {
        float baseJumpForce = CurrentAttribute.jumpForce;
        CurrentAttribute.jumpForce *= buffItemData.multiplierValue;

        yield return new WaitForSeconds(buffItemData.duration);

        CurrentAttribute.jumpForce = baseJumpForce;
    }

    public void ApplyItemValue(EquippableItemData itemData, bool isEquipping)
    {
        switch(itemData.type)
        {
            case EquippableItemType.Speed:
                CurrentAttribute.moveSpeed += isEquipping ? itemData.increaseValue : -itemData.increaseValue;
                break;

            case EquippableItemType.Jump:
                CurrentAttribute.jumpForce += isEquipping ? itemData.increaseValue : -itemData.increaseValue;
                break;
        }
    }
}
