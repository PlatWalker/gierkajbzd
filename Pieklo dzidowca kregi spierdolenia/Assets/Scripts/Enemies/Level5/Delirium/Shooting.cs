using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Shooting : MonoBehaviour
{
    [FormerlySerializedAs("particleSystem")] public ParticleSystem myParticleSystem;
    
    public GameObject explosion;

    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();
    private void Start(){
        myParticleSystem=GetComponent<ParticleSystem>();
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            myParticleSystem.Play();
        }
    }
    private void OnParticleCollision(GameObject other){
        int events = myParticleSystem.GetCollisionEvents(other, colEvents);
        
        for(int i=0;i<events;i++){
            Instantiate(explosion, colEvents[i].intersection, Quaternion.LookRotation(colEvents[i].normal));
        }
    }
    
}
