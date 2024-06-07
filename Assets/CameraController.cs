using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 위치만 따라다니는 카메라 컨트롤러
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 3, -3);


    void Start()
    {
        player = GameObject.FindWithTag(Define.PlayerTag).transform;
    }

    void LateUpdate()
    {
        transform.rotation = player.rotation;

        transform.position = player.position + offset;
    }
}
