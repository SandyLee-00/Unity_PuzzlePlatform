using UnityEngine;

public abstract class ItemObject : MonoBehaviour, IInspectable
{
    public abstract string GetPrompt();
}
