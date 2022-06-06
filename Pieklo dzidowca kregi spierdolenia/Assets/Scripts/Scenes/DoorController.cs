using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator _doorAnimR;
    Animator _doorAnimL;
    private void OnTriggerEnter(Collider other){
        _doorAnimR.SetBool("isOpening",true);
        _doorAnimL.SetBool("isOpening",true);
    }
    private void OnTriggerExit(Collider other){
        _doorAnimR.SetBool("isOpening",false);
        _doorAnimL.SetBool("isOpening",false);
    }
    [SerializeField] private GameObject doorR;
    [SerializeField] private GameObject doorL;
    void Start(){
        _doorAnimR = doorR.transform.GetComponent<Animator>();
        _doorAnimL = doorL.transform.GetComponent<Animator>();
    }
}
