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



    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(),new string[] { });
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    //void OnValidate()
    //{
        //here will be code to handle debuging and balance changes in values
        //for example there have to be check if the AggroRadius is bigger that AttackRadius etc.
    //}

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        if (turnOffAI) return;

        transform.Rotate(0.0f, -90.0f, 0.0f); //reApply Bug of gizmos

        float distanceToMainChar =
            Vector3.Distance(transform.position, MainCharacterTransform.position);


        if (distanceToMainChar < AggroRadius) // checking if main char is visible for enemy
        {
            aggroTimer = (aggroTimer != null && aggroTimer.IsRunning)||aggroCommenced ? aggroTimer : Stopwatch.StartNew();

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
}
