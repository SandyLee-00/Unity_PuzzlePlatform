using UnityEngine;

public class EquippableItem : ItemObject, IInteractable
{
    public EquippableItemData equippableItemData;
    // TODO: 태그로 찾을 수 있도록 수정
    public UIInventory inventory;

    public override string GetPrompt()
    {
        return $"{equippableItemData.itemName}\n{equippableItemData.description}";
    }

    public void Interact()
    {
        inventory.AddItem(equippableItemData);
        Destroy(gameObject);
    }
}
