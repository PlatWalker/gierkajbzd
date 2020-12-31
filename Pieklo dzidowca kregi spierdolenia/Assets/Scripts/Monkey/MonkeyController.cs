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
    [SerializeField] private GameObject projectileObject =  null;

    [SerializeField] private Transform projectileSpawnPoint = null;

    [SerializeField] private float throwPower=1.0f;

    [SerializeField] private float throwTargetHeight = 1.8f;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        string[] ignoredBooleans = new string[] { "spawnProjectile" };
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(),ignoredBooleans);
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    override protected void OnValidate()
    {
        //here will be code to handle debuging and balance changes in values
        //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
        base.OnValidate();
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        float distanceToMainChar = Vector3.Distance(gameObject.transform.position, MainCharacterTransform.position);
        if (distanceToMainChar < AggroRadius)// checking if main char is visible for enemy
        {
            if (distanceToMainChar < AttackRadius) //checking if main char is in attack range
            {
                if (distanceToMainChar < runAwayRadius)
                {
                    //move enemy in opposite direction than main char and with speed debuff
                    UpdateEnemyPosition(new Vector3(gameObject.transform.position.x - MainCharacterTransform.position.x,
                                                    0,
                                                    gameObject.transform.position.z - MainCharacterTransform.position.z),runSpeedDebuff);
                    UpdateEnemyRotation(gameObject.transform.position-MainCharacterTransform.position);
                    easyAnimator.setBooleanTrue("isWalking");
                }
                else
                {
                    UpdateEnemyRotation(MainCharacterTransform.position - gameObject.transform.position );
                    easyAnimator.setBooleanTrue("isAttacking");
                    if (easyAnimator.GetBool("spawnProjectile"))
                    { 
                        GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
                        Vector3 throwDirection = (MainCharacterTransform.position - projectile.transform.position);
                        throwDirection.y += throwTargetHeight;
                        throwDirection *= throwPower;
                        rigidbody.AddForce(throwDirection,ForceMode.Impulse);
                        easyAnimator.SetBoolDirectly("spawnProjectile", false);
                        
                        //Here add some rotatiom of banana
                    }
                    
                }
            }
            else
            {
                //if main char is not in attack range then get closer
                UpdateEnemyRotation(MainCharacterTransform.position - gameObject.transform.position);
                UpdateEnemyPosition(new Vector3(MainCharacterTransform.position.x - gameObject.transform.position.x,
                                                0,
                                                MainCharacterTransform.position.z - gameObject.transform.position.z),
                                                1.0f);
                easyAnimator.setBooleanTrue("isWalking");
            }
        }
        else if (Vector3.Distance(this.gameObject.transform.position, SpawnPoint) > 2.0f) //if main char is not visible and this enemy is far form spawn point then go back to spawn
        {
            UpdateEnemyRotation(new Vector3(SpawnPoint.x, 0, SpawnPoint.z) - gameObject.transform.position);
            UpdateEnemyPosition(new Vector3(SpawnPoint.x - gameObject.transform.position.x,
                                            0,
                                            SpawnPoint.z - gameObject.transform.position.z),
                                            1.0f);
            easyAnimator.setBooleanTrue("isWalking");
        }

        if (CurrentHealth<=0)
        {
            easyAnimator.setBooleanTrue("isDead");
        }
    }
}