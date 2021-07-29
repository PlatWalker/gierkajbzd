using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///by SilverWalker
///</summary>

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 11, -6);
        player = GameManager.Instance.PlayerObject;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
