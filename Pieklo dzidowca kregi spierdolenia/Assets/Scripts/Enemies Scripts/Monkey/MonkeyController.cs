using UnityEngine;
/// <summary>
/// Created By Kumdzio
/// </summary>
public class MonkeyController : EnemyController
{
    [Header("Running from player")]
    [SerializeField] private float runAwayRadius = 5.0f;

    [SerializeField] private float runSpeedModifier = 0.7f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectileObject = null;

    [SerializeField] private Transform projectileSpawnPoint = null;

    [SerializeField] private float throwPower = 1.0f;

    [SerializeField] private float throwTargetHeight = 1.8f;

    private enum MonkeyState
    {
        Idle,
        Chase,
        Attack,
        Run,
        Search,
        Dying,
        AIOff
    }

    private MonkeyState currentState;
    private MonkeyState resumeState;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        string[] ignoredBooleans = new string[] { "spawnProjectile" };
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), ignoredBooleans);
        currentState = MonkeyState.Idle;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    override protected void OnValidate()
    {
        /*
        base.OnValidate();
        if (runAwayRadius >= AttackRadius)
        {
            Debug.Log(transform.name + ": Run Away Radius cannot be bigger or equal to Attack Radius!");
            runAwayRadius = AttackRadius - 0.1f;
        }
        if (runSpeedModifier < 0.1 || runSpeedModifier > 2f)
        {
            Debug.Log(transform.name + ": Run Speed Debuff have to be inside range <0.1,2.0>");
            if (runSpeedModifier < 0.1)
            {
                runSpeedModifier = 0.1f;
            }
            else
            {
                runSpeedModifier = 2f;
            }
        }
        if (projectileObject == null)
        {
            Debug.Log(transform.name + ": Projectile Object for Monkey is not set!");
        }
        if (projectileSpawnPoint == null)
        {
            Debug.Log(transform.name + ": Projectile spawn point for Monkey is not set!");
        }
        if (throwPower <= 0f || throwPower > 10f )
        {
            Debug.Log(transform.name + ": Projectile throw power have to be bigger than 0 and connot be bigger than 10");
            if (throwPower <= 0f)
            {
                throwPower = 0.1f;
            }
            else
            {
                throwPower = 10f;
            }
        }
        */

        //there will be some debug protections some day
        //some day...
    }

    // Update is called once per frame
    override protected void Update()
    {

        HandleLogicPerformaceBoost();
        
        switch (currentState)
        {
            case MonkeyState.AIOff:
                return;
            case MonkeyState.Attack:
                {
                    MoveTo(MainCharacterTransform.position,0,AttackRadius);
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

                    if (updateLogicFrame)
                    {

                    }
                }
                break;
            case MonkeyState.Chase:
                {
                    MoveTo(MainCharacterTransform.position, MovementSpeed, AttackRadius);
                    easyAnimator.SetBooleanTrue("isWalking");

                    if (updateLogicFrame)
                    {
                        
                    }
                }
                break;
            case MonkeyState.Dying:
                {
                    Die();
                }
                break;
            case MonkeyState.Idle:
                {

                    if (updateLogicFrame)
                    {

                    }
                }
                break;
            case MonkeyState.Run:
                {

                    if (updateLogicFrame)
                    {

                    }
                }
                break;
            case MonkeyState.Search:
                {

                    if (updateLogicFrame)
                    {

                    }
                }
                break;
        }













        /*if (distanceToMainChar < AggroRadius)
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
        else if (Vector3.Distance(transform.position, SpawnPoint) > 2.0f)
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
        }*/
    }

    private void GoBackToSpawn()
    {
        //MoveTo(false, normalSpeedModifier, SpawnPoint);
        easyAnimator.SetBooleanTrue("isWalking");
    }

    private void GetCloser()
    {
        
    }

    private void Attack()
    {

        
    }

    protected override bool HandleTriggerByEnemy(Vector3 target)
    {
        return false;
    }

    private void RunFromDanger()
    {
       //MoveTo(true, runSpeedModifier, MainCharacterTransform.position);
        easyAnimator.SetBooleanTrue("isWalking");
    }

    override public void SwitchAI()
    {
        base.SwitchAI();
        if (turnOffAI)
        {
            resumeState = currentState;
            currentState = MonkeyState.AIOff;
        }
        else
        {
            currentState = resumeState;
        }
    }
}