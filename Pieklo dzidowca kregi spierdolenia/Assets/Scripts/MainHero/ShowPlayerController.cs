using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerController : MonoBehaviour
{
    public LayerMask Mask;
    private Material hitedMaterial;
    private int SizeID;

    private void Start()
    {
        SizeID = Shader.PropertyToID("_size");
    }

    void Update()
    {
        Vector3 dir = GameManager.Instance.PlayerObject.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray,out hitinfo, 100, Mask))
        {
            Debug.Log("uderzenie: "+hitinfo.transform.name);
            Renderer renderer = hitinfo.transform.GetComponent<Renderer>();
            if (!renderer)
            {
                if (hitedMaterial)
                {
                    hitedMaterial.SetFloat(SizeID, 0);
                }
                return;
            }
            hitedMaterial = renderer.material;
            hitedMaterial.SetFloat(SizeID, 2);
            if (hitedMaterial.shader.name!="Shader Graphs/show_player")
            {
                hitedMaterial.SetFloat(SizeID, 0);
            }
        }
        else
        {
            if (hitedMaterial)
            {
                hitedMaterial.SetFloat(SizeID, 0);
            }
        }
    }
}
