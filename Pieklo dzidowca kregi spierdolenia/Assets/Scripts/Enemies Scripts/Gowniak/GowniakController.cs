///<summary>
/// Created by Szwagier
///Edited by Kumdzio
///</summary>


using System.Diagnostics;
using UnityEngine;

public class GowniakController : EnemyController
{
    [SerializeField] private float aggroMaxTimeMs = 5000f;

    private bool aggroCommenced = false;

    private Stopwatch aggroTimer;

    private float normalSpeedModifier = 1f;

    GowniakState currentState;
    GowniakState resumeState;

    private enum GowniakState
    {
        Idle,
        Aggro,
        Chase,
        Attack,
        Wander,
        Dying,
        AIOff
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] { });

    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    //void OnValidate()
    //{
    //here will be code to handle debuging and balance changes in values
    //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
    //}

    // Update is called once per frame
    override protected void Update()
    {
        if (CurrentHealth <= 0 && Alive)
        {
            MultiUseTimer = 0f;
            currentState = GowniakState.Dying;
        }

        HandleLogicPerformaceBoost();

        switch (currentState)
        {
            case GowniakState.Aggro:
                {

                }
                break;
            case GowniakState.AIOff:
                {

                }
                break;
            case GowniakState.Attack:
                {

                }
                break;
            case GowniakState.Chase:
                {

                }
                break;
            case GowniakState.Dying:
                {

                }
                break;
            case GowniakState.Idle:
                {

                }
                break;
            case GowniakState.Wander:
                {
                    
                }
                break;
            default:
                UnityEngine.Debug.Log("Some Gowniak is in strange and unrecognized state");
                break;
        }











        //below old style delete when done
        if (turnOffAI) return;

        transform.Rotate(0f, -90.0f, 0.0f); //reApply Bug of gizmos

        float distanceToMainChar =
            Vector3.Distance(transform.position, MainCharacterTransform.position);


        if (distanceToMainChar < AggroRadius) // checking if main char is visible for enemy
        {
            aggroTimer = (aggroTimer != null && aggroTimer.IsRunning) || aggroCommenced ? aggroTimer : Stopwatch.StartNew();

            //MoveTo(false, 0f, MainCharacterTransform.position);

            if (distanceToMainChar < AttackRadius) //checking if main char is in attack range
            {
                easyAnimator.SetBooleanTrue("Attack");

                //attack code goes here

            }
            else if (aggroCommenced)
            {
                easyAnimator.SetBooleanTrue("Move");
                //MoveTo(false, normalSpeedModifier, MainCharacterTransform.position);
            }
            else
            {
                easyAnimator.SetBooleanTrue("InAggroRadius");
                if (aggroTimer != null && aggroTimer.IsRunning && aggroTimer.ElapsedMilliseconds > aggroMaxTimeMs)
                {
                    aggroCommenced = true;
                    aggroTimer = null;
                }
            }
        }
        else if (Vector3.Distance(transform.position, SpawnPoint) > 2f)
        {
            aggroTimer = null; // stop aggroTimer, main char outside of aggro radius
            easyAnimator.SetBooleanTrue("Move");
            // MoveTo(false, normalSpeedModifier, SpawnPoint);
        }
        else
        {
            easyAnimator.ResetAllBooleans();
        }
        transform.Rotate(0.0f, 90.0f, 0.0f); // removing Bug of gizmos
    }

    protected override bool HandleTriggerByEnemy(Vector3 target)
    {
        return false;
    }

    override public void SwitchAI()
    {
        base.SwitchAI();
        if (turnOffAI)
        {
            resumeState = currentState;
            currentState = GowniakState.AIOff;
        }
        else
        {
            currentState = resumeState;

        }
    }
}
