using UnityEngine;

public enum PlatformType
{
    Trap,
    Movable
}

public class PlatformData : ScriptableObject
{
    [Header("Base Info")]
    public PlatformType type;
    public string platformName;
    public string description;
}
