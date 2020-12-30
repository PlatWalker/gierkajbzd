using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///by SilverWalker
///</summary>

public class CameraController : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 11, -6);

    [SerializeField]
    private Vector3 RotationOffset = new Vector3(58, 0, 0);

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(RotationOffset.x, RotationOffset.y, RotationOffset.z);
        transform.position = player.transform.position + offset;
    }
}
