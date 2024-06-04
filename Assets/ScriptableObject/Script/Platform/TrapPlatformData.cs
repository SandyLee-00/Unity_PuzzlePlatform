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
    // 필요한거 사용
    // 데미지 간격
    // public float damageDelay;
    // 데미지 지속시간
    // public float duration;
    // ...
}
