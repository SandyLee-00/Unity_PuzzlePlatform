using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : Platform
{
    public TrapPlatformData trapPlatform;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            Debug.Log("플레이어 데미지");
        }
    }
}
