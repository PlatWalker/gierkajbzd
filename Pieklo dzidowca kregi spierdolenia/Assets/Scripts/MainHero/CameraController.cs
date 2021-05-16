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
    private Vector3 offset;

    private void Awake()
    {
        offset = new Vector3(0, 11, -6);
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
