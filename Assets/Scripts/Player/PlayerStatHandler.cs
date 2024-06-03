using UnityEngine;
/// <summary>
/// Stat을 관리하는 핸들러
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
