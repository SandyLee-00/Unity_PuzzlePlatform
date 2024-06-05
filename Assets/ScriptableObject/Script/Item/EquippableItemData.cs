using UnityEngine;
using UnityEngine.UI;

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
    public GameObject itemPrefab;
    public Sprite icon;
    public float increaseValue;
}
