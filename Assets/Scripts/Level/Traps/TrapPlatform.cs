using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : Platform, IInspectable
{
    public TrapPlatformData trapPlatform;

    //지속 데미지 릴레이 추가

    public override string GetPrompt()
    {
        //if 이게 돌밭이라면
        return $"{trapPlatform.platformName}\n{trapPlatform.description}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHeartStamina playerHeartStamina))
        {
            playerHeartStamina.ChangeHeart(-trapPlatform.damage);
            if(trapPlatform.trapType == TrapPlatformType.Stone)
            {
                //속도 감소
            }
        }
        else
        {
            Debug.LogError("PlayerHeartStamina 컴포넌트를 찾을 수 없습니다.");
        }
    }
}
