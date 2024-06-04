using UnityEngine;

public class EquippableItem : ItemObject, IInteractable
{
    public EquippableItemData equippableItemData;

    public override string GetPrompt()
    {
        return $"{equippableItemData.itemName}\n{equippableItemData.description}";
    }

    public void Interact()
    {
        // TODO: 데이터 처리
        Destroy(gameObject);
    }
}
