using UnityEngine;

public class EquippableItem : ItemObject, IInteractable
{
    public EquippableItemData equippableItemData;
    private UIInventory _inventory;

    private void Awake()
    {
        _inventory = GameObject.FindWithTag(Define.InventoryTag).GetComponent<UIInventory>();
    }

    public override string GetPrompt()
    {
        return $"{equippableItemData.itemName}\n{equippableItemData.description}";
    }

    public void Interact()
    {
        _inventory.AddItem(equippableItemData);
        Destroy(gameObject);
    }
}
