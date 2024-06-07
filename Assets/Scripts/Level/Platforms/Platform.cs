using UnityEngine;

public abstract class Platform : MonoBehaviour, IInspectable
{
    public abstract string GetPrompt();
}
