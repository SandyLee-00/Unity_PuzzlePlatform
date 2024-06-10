using UnityEngine;

public enum TrapPlatformType
{
    Fire,
    Spike,
    Stone
}

[CreateAssetMenu(fileName = "New TrapPlatformData", menuName = "SOData/PlatformData/TrapPlatform")]
public class TrapPlatformData : PlatformData
{
    [Header("Trap Info")]
    public TrapPlatformType trapType;
    public int damage;
}
