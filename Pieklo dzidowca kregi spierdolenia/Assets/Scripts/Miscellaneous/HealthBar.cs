///<summary>
///Created by Kumdzio
///</summary>


using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform healthBarTransform;
    private IDamageable parentScript;
    private SpriteRenderer healthBarRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (!(this.transform.parent.gameObject.TryGetComponent<IDamageable>(out parentScript))) Destroy(this.gameObject);
        cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        healthBarTransform = this.gameObject.GetComponent<Transform>();
        healthBarRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = parentScript.GetHealthPercentage();
        if (percentage < 0) percentage = 0;
        healthBarRenderer.size = new Vector2((0.16f * percentage), healthBarRenderer.size.y);
        healthBarTransform.LookAt(new Vector3(this.GetComponent<Transform>().position.x, cameraTransform.position.y, cameraTransform.position.z));
    }
}
