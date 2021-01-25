using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootControl : MonoBehaviour
{
    GameObject projectileObject;
    [SerializeField] private float throwPower=1.0f;
    private float throwTargetHeight=1.0f;

    // Start is called before the first frame update
    void Start()
    {
        projectileObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectileObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Rigidbody rigidbody = projectileObject.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                GameObject projectile = Instantiate(projectileObject, (transform.position + new Vector3(0,2,0)), transform.rotation);

                Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
                Vector3 throwDirection = (hit.point - projectile.transform.position);
                Debug.Log(hit.point);

                throwDirection.y += throwTargetHeight;
                throwDirection = throwDirection.normalized;
                throwDirection *= throwPower;
                rigidbody.AddForce(throwDirection, ForceMode.Impulse);
            }
        }
    }
}
