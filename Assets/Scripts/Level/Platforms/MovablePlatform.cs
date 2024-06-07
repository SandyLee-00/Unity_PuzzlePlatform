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
    private Vector3 moveDirection;  //이동하려는 방향

    private bool isReverse;

    private void Start()
    {
        startPos = transform.position;  //원래 자리로 돌아오기 위한 위치

        endPos = transform.TransformPoint(new Vector3(maxDistance, 0f, 0f));
        //Debug.Log(Vector3.Distance(startPos, endPos));

        if (moveData.movableType == MovablePlatformType.Horizontal)
            moveDirection = Vector3.right;
        else if (moveData.movableType == MovablePlatformType.Vertical)
            moveDirection = Vector3.up;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform() //발판 움직임
    {
        int moveReverse = isReverse ? -1 : 1;

        //현재 위치에서 최대 거리보다 넘어가면
        if (Vector3.Distance(startPos, transform.position) > maxDistance && !isReverse)
            WaitToReverse();
        else if(Vector3.Distance(transform.position, startPos) < 0.1f && isReverse)
            WaitToReverse();
        else
        {
            transform.Translate(moveDirection * moveData.platformSpeed * moveReverse * Time.deltaTime);
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
