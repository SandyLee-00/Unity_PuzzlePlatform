using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHealthMana player))
        {
            if (player.ChangeHP(-1))
            {
                Debug.Log("처음부터 시작");
                player.transform.position = new Vector3(-4, 3, -2);
            }
            else
            {
                Debug.Log("게임오버");
            }
        }
    }
}
