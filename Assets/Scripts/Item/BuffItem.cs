using UnityEngine;

public class BuffItem : ItemObject
{
    public BuffItemData buffItemData;

    public override string GetPrompt()
    {
        return $"{buffItemData.itemName}\n{buffItemData.description}";
    }

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

                //case BuffItemType.Speed:
                //    점프 처리
                //    Destroy(gameObject);
                //    break;
            }
        }
    }
}
