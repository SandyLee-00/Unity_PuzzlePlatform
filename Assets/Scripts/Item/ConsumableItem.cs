using UnityEngine;

public class ConsumableItem : ItemObject
{
    public ConsumableItemData consumableItemData;
    public override string GetPrompt()
    {
        return $"{consumableItemData.itemName}\n{consumableItemData.description}";
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //}
}
