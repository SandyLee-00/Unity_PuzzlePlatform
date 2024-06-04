using UnityEngine;

public enum MovablePlatformType
{
    Horizontal,
    Vertical
}

[CreateAssetMenu(fileName = "New MovablePlatformData", menuName = "SOData/PlatformData/MovablePlatform")]
public class MovablePlatformData : PlatformData
{
    [Header("Movable Platform Data")]
    public MovablePlatformType movableType;
    public float platformSpeed;
}
