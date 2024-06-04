using UnityEngine;

public class ConsumableItem : ItemObject
{
    public ConsumableItemData consumableItemData;

    public override string GetPrompt()
    {
        return $"{consumableItemData.itemName}\n{consumableItemData.description}";
    }

    // TODO: switch 사용 안 하는 방식으로 수정
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealthMana playerStatus))
        {
            switch (consumableItemData.type)
            {
                case ConsumableItemType.Health:
                    playerStatus.ChangeHP(consumableItemData.increaseValue);
                    Destroy(gameObject);
                    break;

                case ConsumableItemType.Stamina:
                    playerStatus.ChangeMP(consumableItemData.increaseValue);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
