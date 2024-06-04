using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : Platform
{
    public TrapPlatformData trapPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHealthMana playerState))
        {
            Debug.Log("플레이어 데미지");
            playerState.ChangeHP(-trapPlatform.damage);

            //트랩마다 종류가 있을텐데 trapPlatform에서 타입을 확인하고 결정
        }
    }
}
