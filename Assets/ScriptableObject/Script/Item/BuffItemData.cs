using UnityEngine;

public enum BuffItemType
{
    Speed,
    Jump
}

[CreateAssetMenu(fileName = "New BuffItemData", menuName = "SOData/ItemData/BuffItem")]
public class BuffItemData : ItemData
{
    [Header("Buff Info")]
    public BuffItemType type;
    public float duration;
    public float multiplierValue;
}
