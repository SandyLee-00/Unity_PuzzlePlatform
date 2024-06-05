using UnityEngine;

public enum ConsumableItemType
{
    Health,
    Stamina
}

[CreateAssetMenu(fileName = "New ConsumableItemData", menuName = "SOData/ItemData/ConsumableItem")]
public class ConsumableItemData : ItemData
{
    [Header("Consum Info")]
    public ConsumableItemType type;
    public int increaseValue;
}

