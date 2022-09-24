using System.Collections;
using System.Collections.Generic;
using jbzd;
using jbzdy.Player;
using UnityEngine;
using Zenject;

///<summary>
///by SilverWalker
///</summary>

public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 0.05f;
    private Vector3 offset;
    private GameObject player;
    private Vector3 velocity = Vector3.zero;
    
    private PlayerController _playerController;
        
    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    private void Start()
    {
        offset = new Vector3(0, 11, -6);
        player = _playerController.gameObject;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
