using UnityEngine;

/// <summary>
/// Player HP, MP 를 제외한 모든 스탯을 관리한다.
/// </summary>
public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField]
    public PlayerStat CurrentStat { get; private set; }

    private void Awake()
    {
        CurrentStat = new PlayerStat();
    }
}
