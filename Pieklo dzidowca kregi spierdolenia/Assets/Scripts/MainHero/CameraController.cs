using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position;
        offset.z -= 6 ;
    }
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
