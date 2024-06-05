using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : Platform, IInspectable
{
    public TrapPlatformData trapPlatform;

    //지속 데미지 릴레이 추가

    public string GetPrompt()
    {
        return "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHealthMana playerState))
        {
            //체력 감소 (IDamagable이 추가되면 이동할 예정)
            if (playerState.ChangeHP(-trapPlatform.damage))
            {
                Debug.Log(playerState.CurrentHP);
            }
            else
            {
                //gameOver
            }
            //트랩마다 종류가 있을텐데 trapPlatform에서 타입을 확인하고 결정
        }
    }
}
