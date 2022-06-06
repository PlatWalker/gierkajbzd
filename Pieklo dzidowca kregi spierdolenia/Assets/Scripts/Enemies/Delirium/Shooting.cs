using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public ParticleSystem particleSystem;
    
    public GameObject explosion;

    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();
    private void Start(){
        particleSystem=GetComponent<ParticleSystem>();
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            particleSystem.Play();
        }
    }
    private void OnParticleCollision(GameObject other){
        int events = particleSystem.GetCollisionEvents(other, colEvents);
        
        for(int i=0;i<events;i++){
            Instantiate(explosion, colEvents[i].intersection, Quaternion.LookRotation(colEvents[i].normal));
        }
    }
    
}
