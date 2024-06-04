using UnityEngine;

public enum EquippableItemType
{
    Speed,
    Jump
}

[CreateAssetMenu(fileName = "New EquippableItemData", menuName = "SOData/ItemData/EquippableItem")]
public class EquippableItemData : ItemData
{
    [Header("Equip Info")]
    public EquippableItemType type;
    public float increaseValue;
}
