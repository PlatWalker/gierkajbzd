using System;
using UnityEngine;

namespace jbzd.MainHero.DebugScripts
{
    [Obsolete("Stary skrypt kumdzia do debugowania")]
    public class ShootControl : MonoBehaviour
    {
        GameObject projectileObject;
        [Header("Throw settings")]
        [SerializeField] private float throwPower=1.0f;
        [SerializeField] private float throwTargetHeight=1.0f;
        [Header("Bullet properties")]
        [SerializeField] private float timeToDestruction = 25.0f;
        [SerializeField] public int damageAmount = 10;
        [SerializeField] public DamageType typeOfDamage = DamageType.Dystansowa;
        [SerializeField] public float critChance = 0.0f;
        [SerializeField] public float critMultiplier = 1.0f;


        // Start is called before the first frame update
        void Start()
        {
            projectileObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            projectileObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            projectileObject.transform.position = transform.position - new Vector3(0, 3f, 0);
            Rigidbody rigidbody = projectileObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.useGravity = false;
            rigidbody.mass = 0.1f;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject projectile = Instantiate(projectileObject, (transform.position + new Vector3(0,2,0)), transform.rotation);

                    projectile.AddComponent<BulletController>();
                    projectile.GetComponent<BulletController>().SetUp(timeToDestruction, damageAmount, typeOfDamage, critMultiplier, critChance);

                    Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
                    Vector3 throwDirection= (hit.point - projectile.transform.position);
                    throwDirection.y += throwTargetHeight;


                    throwDirection = throwDirection.normalized;
                    throwDirection *= throwPower;
                    rigidbody.AddForce(throwDirection, ForceMode.Impulse);
                }
            }
        }
    }
}
