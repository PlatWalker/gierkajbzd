using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform healthBarTransform;
    private Damageable parentScript;
    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        healthBarTransform = this.gameObject.GetComponent<Transform>();
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
        if (!(this.transform.parent.gameObject.TryGetComponent<Damageable>(out parentScript))) Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        renderer.size = new Vector2((0.16f * parentScript.getHealthPercentage()), renderer.size.y);
        healthBarTransform.LookAt(new Vector3(this.GetComponent<Transform>().position.x, cameraTransform.position.y, cameraTransform.position.z));
    }
}
