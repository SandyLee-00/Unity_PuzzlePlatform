using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : Platform
{
    public MovablePlatformData moveData;
    public float maxDistance;   //최대 갈 수 있는 거리
    public float watingTime;    //다시 돌아가기 까지 대기하는 시간

    private float nowTime = 0;
    private Vector3 startPos;
    private Vector3 endPos;

    private bool isReverse;

    private void Start()
    {
        startPos = transform.localPosition;  //원래 자리로 돌아오기 위한 위치

        if(moveData.movableType == MovablePlatformType.Horizontal)
            endPos.x = transform.localPosition.x + maxDistance;
        else if(moveData.movableType == MovablePlatformType.Vertical)
            endPos.y = transform.localPosition.y + maxDistance;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform() //발판 움직임
    {
        int moveReverse = isReverse ? -1 : 1;

        if (moveData.movableType == MovablePlatformType.Horizontal)
        {
            //x축 이동
            //현재 위치에서 최대 거리보다 넘어가면
            if (transform.position.x >= endPos.x && !isReverse)
                WaitToReverse();
            else if(transform.position.x <= startPos.x && isReverse)
                WaitToReverse();
            else
            {
                transform.Translate(Vector3.right * moveData.platformSpeed * moveReverse * Time.deltaTime);
            }
        }
        else if (moveData.movableType == MovablePlatformType.Vertical)
        {
            //y축 이동
            if (transform.position.y >= endPos.y && !isReverse)
                WaitToReverse();
            else if (transform.position.y <= startPos.y && isReverse)
                WaitToReverse();
            else
            {
                transform.Translate(Vector3.up * moveData.platformSpeed * moveReverse * Time.deltaTime);
            }
        }
    }

    private void WaitToReverse()    //바꾸기 전까지 대기
    {
        nowTime += Time.deltaTime;
        if (nowTime >= watingTime)
        {
            nowTime = 0;
            ChangeDirection();
        }
    }

    private void ChangeDirection()  //방향 전환
    {
        isReverse = !isReverse;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement movement))
        {
            movement.transform.SetParent(transform);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerMovement movement))
        {
            movement.transform.SetParent(null);
        }
    }
}
