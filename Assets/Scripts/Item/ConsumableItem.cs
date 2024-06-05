using UnityEngine;

public class ConsumableItem : ItemObject
{
    public ConsumableItemData consumableItemData;

    public override string GetPrompt()
    {
        return $"{consumableItemData.itemName}\n{consumableItemData.description}";
    }

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
                    playerStatus.ChangeStamina(consumableItemData.increaseValue);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
