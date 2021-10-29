using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearCameraIn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TurnOffMesh(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        TurnOnMesh(other.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        TurnOffMesh(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        TurnOnMesh(collision.gameObject);
    }

    private void TurnOffMesh(GameObject hitedObject)
    {
        MeshRenderer renderer = hitedObject.GetComponent<MeshRenderer>();
        if (renderer)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
        else
        {
            Debug.Log("Kolizja z obiektem bez MeshRender");
        }
    }
    private void TurnOnMesh(GameObject hitedObject)
    {
        MeshRenderer renderer = hitedObject.GetComponent<MeshRenderer>();
        if (renderer)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else
        {
            Debug.Log("Kolizja z obiektem bez MeshRender");
        }
    }
}
