///<summary>
///Created By Kumdzio
///</summary>

using UnityEngine;

public class MonkeyController : MonoBehaviour,Damageable
{
    [SerializeField] public Transform mainCharacterTransform;
    [SerializeField] private float movementSpeed = 0.14f;
    [SerializeField] private float aggroRadius = 20.0f;
    [SerializeField] private float attackRadius = 8.5f;
    [SerializeField] private float runRadius = 5.0f;
    [SerializeField] private float runSpeedDebuff = 0.7f;
    [SerializeField] private float rotationSpeed = 0.15f;
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float throwPower=1.0f;
    [SerializeField] private int maxHealth = 100;


    private Vector3 spawnPoint;
    private Animator monkeyAnimator;
    EasyAnimatorController easyMonkeyAnimator;
    [SerializeField]private int currentHealth; //zminić by nie było serialize po testach


    // Start is called before the first frame update
    void Start()
    {
        monkeyAnimator = GetComponent<Animator>();
        string[] ignoredBooleans = new string[] { "spawnProjectile" };
        easyMonkeyAnimator = new EasyAnimatorController(GetComponent<Animator>(),ignoredBooleans);
        spawnPoint = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isWalking = false, isAttacking = false, isDead = false;
        float distanceToMainChar = Vector3.Distance(this.gameObject.transform.position, mainCharacterTransform.position);
        if (distanceToMainChar < aggroRadius)// checking if main char is visible for enemy
        {
            if (distanceToMainChar < attackRadius) //checking if main char is in attack range
            {
                if (distanceToMainChar < runRadius)
                {
                    //move enemy in opposite direction than main char and with speed debuff
                    UpdateEnemyPosition(new Vector3(this.gameObject.transform.position.x - mainCharacterTransform.position.x, 0, this.gameObject.transform.position.z - mainCharacterTransform.position.z),runSpeedDebuff);
                    UpdateEnemyRotation(this.transform.position-mainCharacterTransform.position);
                    isWalking = true;
                }
                else
                {
                    UpdateEnemyRotation(mainCharacterTransform.position - this.transform.position );
                    isAttacking = true;
                    if (monkeyAnimator.GetBool("spawnProjectile"))
                    {
                        //tutaj tworze pocisk
                        GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
                        Vector3 throwDirection = (mainCharacterTransform.position - projectile.transform.position);
                        throwDirection.y += 2;
                        throwDirection *= throwPower;
                        rigidbody.AddForce(throwDirection,ForceMode.Impulse);
                        monkeyAnimator.SetBool("spawnProjectile", false);
                        
                        //tutaj dodać rzut bananem - żeby leciał i się kręcił.
                    }
                    //attacking code goes here
                }
            }
            else
            {
                //if main char is not in attack range then get closer
                UpdateEnemyRotation(mainCharacterTransform.position - this.transform.position);
                UpdateEnemyPosition(new Vector3(mainCharacterTransform.position.x - this.gameObject.transform.position.x, 0, mainCharacterTransform.position.z - this.gameObject.transform.position.z));
                isWalking = true;
            }
        }
        else if (Vector3.Distance(this.gameObject.transform.position, spawnPoint) > 2.0f) //if main char is not visible and this enemy is far form spawn point then go back to spawn
        {
            UpdateEnemyRotation(new Vector3(spawnPoint.x, 0, spawnPoint.z)-this.transform.position);
            UpdateEnemyPosition(new Vector3(spawnPoint.x - this.gameObject.transform.position.x, 0, spawnPoint.z - this.gameObject.transform.position.z));
            isWalking = true;
        }

        if (isAttacking)
        {
            //monkeyAnimator.SetBool("isAttacking", true);
            //monkeyAnimator.SetBool("isWalking", false);
            easyMonkeyAnimator.setBooleanTrue("isAttacking");
        }
        else if(isWalking)
        {
            //monkeyAnimator.SetBool("isAttacking", false);
            //monkeyAnimator.SetBool("isWalking", true);
            easyMonkeyAnimator.setBooleanTrue("isWalking");
        }
        else
        {
            easyMonkeyAnimator.ResetAllBooleans();
            //monkeyAnimator.SetBool("isAttacking", false);
            //monkeyAnimator.SetBool("isWalking", false);
        }

        if (isDead)
        {
            //monkeyAnimator.SetBool("isDead", isDead);
            easyMonkeyAnimator.setBooleanTrue("isDead");
        }
    }

    void UpdateEnemyPosition(Vector3 direction,float speedDebuff)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * movementSpeed * speedDebuff;
    }
    void UpdateEnemyPosition(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        this.gameObject.transform.position += direction * movementSpeed;
    }

    void UpdateEnemyRotation(Vector3 direction)
    {
        Vector3 newDirection = Vector3.RotateTowards(this.gameObject.transform.forward, direction, rotationSpeed, 0.0f);
        this.gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public float getHealthPercentage()
    {
        return (float)this.currentHealth / this.maxHealth;
    }
}