using UnityEngine;

public class TrapPlatform : Platform
{
    public TrapPlatformData trapPlatform;

    //지속 데미지 릴레이 추가

    public override string GetPrompt()
    {
        return $"{trapPlatform.platformName}\n{trapPlatform.description}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHeartStamina playerHeartStamina))
        {
            playerHeartStamina.ChangeHeart(-trapPlatform.damage);
        }
        else
        {
            Debug.LogError("PlayerHeartStamina 컴포넌트를 찾을 수 없습니다.");
        }
    }
}
