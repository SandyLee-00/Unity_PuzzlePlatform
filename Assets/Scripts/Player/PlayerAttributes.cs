
/// <summary>
/// 캐릭터의 세팅을 정의하는 클래스
/// </summary>
[System.Serializable]
public class PlayerAttributes
{
    public int maxHeart = 3;

    public float jumpForce = 2.5f;

    public float moveSpeed = 5;

    public float maxStamina = 200;
    public float runMultiplier = 2;
    public float costStaminaRun = 1;

    public float heartChangeDelay = 0.5f;
    public float staminaChangeDelay = 0.5f;
    public float staminaFillAmount = 5f;

}
