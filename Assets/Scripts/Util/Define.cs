/// <summary>
/// Define 모음
/// </summary>
public class Define
{
    public enum Sound
    {
        Bgm = 0,
        Effect,
        Max,
    }

    public enum Scene
    {
        Title = 0,
        Play,
        Max,
    }

    public const string GroundTag = "Ground";
    public const string PlayerTag = "Player";
    public const string InventoryTag = "Inventory";

    public const string GroundLayer = "Ground";
    public const string InteractableLayer = "Interactable";
}
