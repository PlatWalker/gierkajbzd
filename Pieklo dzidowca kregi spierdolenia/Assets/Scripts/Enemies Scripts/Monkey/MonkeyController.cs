///<summary>
///Created By Kumdzio
///</summary>

using UnityEngine;

public class MonkeyController : EnemyController
{
    [Header("Running from player")]
    [SerializeField] private float runAwayRadius = 5.0f;

    [SerializeField] private float runSpeedDebuff = 0.7f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectileObject = null;

    [SerializeField] private Transform projectileSpawnPoint = null;

    [SerializeField] private float throwPower = 1.0f;

    [SerializeField] private float throwTargetHeight = 1.8f;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        string[] ignoredBooleans = new string[] { "spawnProjectile" };
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), ignoredBooleans);
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    override protected void OnValidate()
    {
        base.OnValidate();
        if (runAwayRadius >= AttackRadius)
        {
            Debug.Log("Run Away Radius cannot be bigger or equal to Attack Radius!");
            runAwayRadius = AttackRadius - 0.1f;
        }
        if (runSpeedDebuff < 0.1 || runSpeedDebuff > 2f)
        {
            Debug.Log("Run Speed Debuff have to be inside range <0.1,2.0>");
            if (runSpeedDebuff < 0.1)
            {
                runSpeedDebuff = 0.1f;
            }
            else
            {
                runSpeedDebuff = 2f;
            }
        }
        if (projectileObject == null)
        {
            Debug.Log("Projectile Object for Monkey is not set!");
        }
        if (projectileSpawnPoint == null)
        {
            Debug.Log("Projectile spawn point for Monkey is not set!");
        }
        if (throwPower <= 0f || throwPower > 10f )
        {
            Debug.Log("Projectile throw power have to be bigger than 0 and connot be bigger than 10");
            if (throwPower <= 0f)
            {
                throwPower = 0.1f;
            }
            else
            {
                throwPower = 10f;
            }
        }
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        if (turnOffAI) return;
        float distanceToMainChar = Vector3.Distance(gameObject.transform.position, MainCharacterTransform.position);
        if (distanceToMainChar < AggroRadius)
        {
            if (distanceToMainChar < AttackRadius)
            {
                if (distanceToMainChar < runAwayRadius)
                {
                    RunFromDanger();
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                GetCloser();
            }
        }
        else if (Vector3.Distance(this.gameObject.transform.position, SpawnPoint) > 2.0f)
        {
            GoBackToSpawn();
        }
        else
        {
            easyAnimator.ResetAllBooleans();
        }

        if (CurrentHealth<=0)
        {
            easyAnimator.SetBooleanTrue("isDead");
        }
    }

    private void GoBackToSpawn()
    {
        Move(false, 1f, SpawnPoint);
        easyAnimator.SetBooleanTrue("isWalking");
    }

    private void GetCloser()
    {
        Move(false, 1f, MainCharacterTransform.position);
        easyAnimator.SetBooleanTrue("isWalking");
    }

    private void Attack()
    {
        Move(false, 0f, MainCharacterTransform.position);
        easyAnimator.SetBooleanTrue("isAttacking");
        if (easyAnimator.GetBoolean("spawnProjectile"))
        {
            GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
            Vector3 throwDirection = (MainCharacterTransform.position - projectile.transform.position);
            throwDirection.y += throwTargetHeight;
            throwDirection *= throwPower;
            rigidbody.AddForce(throwDirection, ForceMode.Impulse);
            easyAnimator.SetBooleanDirectly("spawnProjectile", false);

            //Here add some rotatiom of projectile
        }
    }

    private void RunFromDanger()
    {
        Move(true, runSpeedDebuff, MainCharacterTransform.position);
        easyAnimator.SetBooleanTrue("isWalking");
    }
}