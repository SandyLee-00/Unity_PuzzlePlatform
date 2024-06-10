using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHole : MonoBehaviour
{
    public Transform startPos;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Define.PlayerTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerHeartStamina playerHeartStamina))
        {
            if (playerHeartStamina.ChangeHeart(-1))
            {
                Debug.Log("처음부터 시작");
                StartCoroutine(GoToStartPosition(1f));
            }
            else
            {
                Debug.Log("게임오버");
                //첫 화면으로 돌아가기
            }
        }
    }

    private IEnumerator GoToStartPosition(float second)
    {
        yield return new WaitForSeconds(second);

        player.transform.position = startPos.position; //시작 지점으로 추가
        player.transform.rotation = startPos.rotation;
    }
}
