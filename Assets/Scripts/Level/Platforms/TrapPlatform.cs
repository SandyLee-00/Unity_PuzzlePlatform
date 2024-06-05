using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : Platform
{
    public TrapPlatformData trapPlatform;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHeartStamina playerState))
        {
            Debug.Log(playerState.CurrentHeart);
            playerState.ChangeHeart(-trapPlatform.damage);
            Debug.Log(playerState.CurrentHeart);
            //트랩마다 종류가 있을텐데 trapPlatform에서 타입을 확인하고 결정
        }
    }
}
