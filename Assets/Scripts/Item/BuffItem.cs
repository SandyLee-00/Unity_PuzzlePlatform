using UnityEngine;

public class BuffItem : ItemObject
{
    public BuffItemData buffItemData;

    public override string GetPrompt()
    {
        return $"{buffItemData.itemName}\n{buffItemData.description}";
    }

    // TODO: switch 사용 안 하는 방식으로 수정
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerAttributeHandler attributeHandler))
        {
            switch (buffItemData.type)
            {
                case BuffItemType.Speed:
                    attributeHandler.OnSpeedBuff(buffItemData);
                    Destroy(gameObject);
                    break;

                case BuffItemType.Jump:
                    attributeHandler.OnJumpBuff(buffItemData);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
