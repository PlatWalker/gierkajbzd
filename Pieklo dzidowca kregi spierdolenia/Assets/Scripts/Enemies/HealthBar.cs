///<summary>
///Created by Kumdzio
///</summary>


using jbzd.Common.Interfaces;
using System.Linq;
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
        //there should be manager to get camera from
        cameraTransform = GameObject.FindGameObjectsWithTag("MainCamera").OfType<GameObject>().ToList().First(x=>x.name == "Main Camera").GetComponent<Transform>();
        healthBarTransform = this.gameObject.GetComponent<Transform>();
        healthBarRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBarRenderer is null)
        {
            Debug.Log("Nie odnaleziono renderera Healthbara - próbuję znowu");
            healthBarRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            return;
        }
        float percentage = (parentScript.CurrentHealth*1.0f)/(parentScript.MaximumHealth*1.0f);
        if (percentage < 0) percentage = 0;
        healthBarRenderer.size = new Vector2((0.16f * percentage), healthBarRenderer.size.y);
        healthBarTransform.LookAt(new Vector3(this.GetComponent<Transform>().position.x, cameraTransform.position.y, cameraTransform.position.z));
    }
}
