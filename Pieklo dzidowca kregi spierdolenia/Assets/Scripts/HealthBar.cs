///<summary>
///Created by Kumdzio
///</summary>


using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform healthBarTransform;
    private Damageable parentScript;
    private SpriteRenderer healthBarRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (!(this.transform.parent.gameObject.TryGetComponent<Damageable>(out parentScript))) Destroy(this.gameObject);
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        healthBarTransform = this.gameObject.GetComponent<Transform>();
        healthBarRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBarRenderer.size = new Vector2((0.16f * parentScript.getHealthPercentage()), healthBarRenderer.size.y);
        healthBarTransform.LookAt(new Vector3(this.GetComponent<Transform>().position.x, cameraTransform.position.y, cameraTransform.position.z));
    }
}
