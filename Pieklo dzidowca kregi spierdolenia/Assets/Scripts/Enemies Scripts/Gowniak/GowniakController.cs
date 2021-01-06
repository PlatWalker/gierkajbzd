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

    private Stopwatch _aggroTimer;



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

        gameObject.transform.Rotate(0.0f, -90.0f, 0.0f); //reApply Bug of gizmos

        float distanceToMainChar =
            Vector3.Distance(gameObject.transform.position, MainCharacterTransform.position);


        if (distanceToMainChar < AggroRadius) // checking if main char is visible for enemy
        {
            _aggroTimer = (_aggroTimer != null && _aggroTimer.IsRunning)||aggroCommenced ? _aggroTimer : Stopwatch.StartNew();

            Move(false, 0f, MainCharacterTransform.position);

            if (distanceToMainChar < AttackRadius) //checking if main char is in attack range
            {
                easyAnimator.SetBooleanTrue("Attack");

                //attack code goes here

            }
            else if (aggroCommenced)
            {
                easyAnimator.SetBooleanTrue("Move");
                Move(false, 1f, MainCharacterTransform.position);
            }
            else
            {
                easyAnimator.SetBooleanTrue("InAggroRadius");
                if (_aggroTimer != null && _aggroTimer.IsRunning && _aggroTimer.ElapsedMilliseconds > aggroMaxTimeMs)
                {
                    aggroCommenced = true;
                    _aggroTimer = null;
                }
            }
        }
        else if (Vector3.Distance(gameObject.transform.position, SpawnPoint) > 2f)
        {
            _aggroTimer = null; // stop aggroTimer, main char outside of aggro radius
            easyAnimator.SetBooleanTrue("Move");
            Move(false, 1f, SpawnPoint);
        }
        else
        {
            easyAnimator.ResetAllBooleans();
        }
        gameObject.transform.Rotate(0.0f, 90.0f, 0.0f); // removing Bug of gizmos
    }
}
