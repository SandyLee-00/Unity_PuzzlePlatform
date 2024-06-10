using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHole : MonoBehaviour
{
    public Transform startPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHeartStamina player))
        {
            player.ChangeHeart(-1);
            Debug.Log("처음부터 시작");
            player.transform.position = startPos.position; //시작 지점으로 추가
            player.transform.rotation = startPos.rotation;
        }
    }
}
