using UnityEngine;

/// <summary>
/// Player 현재 HP, MP 를 제외한 모든 스탯을 관리한다.
/// </summary>
public class PlayerAttributeHandler : MonoBehaviour
{
    [SerializeField]
    public PlayerAttributes CurrentAttribute { get; private set; }

    private void Awake()
    {
        CurrentAttribute = new PlayerAttributes();
    }
}
