using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : Platform
{
    public TrapPlatformData trapPlatform;

    //지속 데미지 릴레이 추가

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHealthMana playerState))
        {
            Debug.Log(playerState.CurrentHP);
            playerState.ChangeHP(-trapPlatform.damage);
            Debug.Log(playerState.CurrentHP);
            //트랩마다 종류가 있을텐데 trapPlatform에서 타입을 확인하고 결정
        }
    }
}
