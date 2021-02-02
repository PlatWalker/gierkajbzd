///<summary>
///Created by Kumdzio
///</summary>

using UnityEngine;

public class DoomerController : EnemyController
{
    [Header("Special Attack")]
    [SerializeField] private float movementRushSpeedModifier=1.05f;

    [SerializeField] private float jumpSpeed = 1.08f;

    [SerializeField] private float firstAttackRadius = 4.0f;

   
    private Vector3 jumpDirection = Vector3.zero;

    public static bool HasDoneAggro { get; set; }

    private bool hasDoneSpecialAttack;
    
    private const float normalSpeedModifier = 1f;



    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] {"shouldUseSecondAttack"});
        hasDoneSpecialAttack = false;
    }

    //OnValidate is called when script is loaded and everytime when value is changed
    //in the inspector
    override protected void OnValidate()
    {
        base.OnValidate();

        if (movementRushSpeedModifier <= 0f || movementRushSpeedModifier >5f)
        {

            Debug.Log(transform.name + ": Movement Rush Speed Modifier have to be in range (0,5>");

            if (movementRushSpeedModifier <= 0f)
            {
                movementRushSpeedModifier = 0.1f;
            }
            else
            {
                movementRushSpeedModifier = 5f;
            }
        }

        if (jumpSpeed < 1f || jumpSpeed > 5f)
        {

            Debug.Log(transform.name + ": Jump Speed have to be in range <1,5>");

            if (jumpSpeed < 1f)
            {
                jumpSpeed = 1;
            }
            else
            {
                jumpSpeed = 5;
            }
        }
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        HandleDying();

        if (turnOffAI || !Alive ) return;


        float distanceToMainChar = Vector3.Distance(transform.position, MainCharacterTransform.position);

        if (distanceToMainChar < AggroRadius && distanceToMainChar > AttackRadius && PlayerVisible())// checking if main char is visible for enemy and should not attack
        {
            if (HasDoneAggro)
            {

                if (distanceToMainChar <= firstAttackRadius && !hasDoneSpecialAttack) //checking if should start special attack
                {
                    if (jumpDirection == Vector3.zero) 
                    {
                        jumpDirection = MainCharacterTransform.position; //saving jump direction so enemy cannot change direction in air
                            
                    }

                    if (Vector3.Distance(jumpDirection, gameObject.transform.position) > AttackRadius)
                    {
                        MoveTo(false, jumpSpeed, jumpDirection);
                        easyAnimator.SetBooleanTrue("isJumping");
                    }
                    else
                    {
                        hasDoneSpecialAttack = true;
                    }
                }
                else if(hasDoneSpecialAttack)
                {
                    //if attack has been made just move in normal speed to player
                    MoveTo(false, normalSpeedModifier, MainCharacterTransform.position);
                    easyAnimator.SetBooleanTrue("isWalking");
                }
                else
                {
                    //when attack has been not made and cannot be made yet move to player in charge
                    MoveTo(false, movementRushSpeedModifier, MainCharacterTransform.position);
                    easyAnimator.SetBooleanTrue("isRunning");
                }

            }
            else
            {
                easyAnimator.SetBooleanTrue("isAggroing");
                MoveTo(false, 0f, MainCharacterTransform.position);
            }            
        }
        else if (distanceToMainChar <= AttackRadius && PlayerVisible())
        {
            if (distanceToMainChar > 0.3)
            {
                MoveTo(false, 0.0f, MainCharacterTransform.position);
            }
            easyAnimator.SetBooleanTrue("isAttacking");
            //tutaj zadawanie obrażeń - collider i te sprawy
            //dodać tutaj sprawdzenie czy zakończono atak specjalnt i jesli tak to normalne obrażenia a jak nie to dodatkowe obrażenia
        }
        else if(TriggeredByAttack)
        {
            

            if (Vector3.Distance(GoToPoint, transform.position) > 1f)
            {
                if (hasDoneSpecialAttack)
                {
                    MoveTo(false, normalSpeedModifier, GoToPoint);
                    easyAnimator.SetBooleanTrue("isWalking");
                }
                else
                {
                    MoveTo(false, movementRushSpeedModifier, GoToPoint);
                    easyAnimator.SetBooleanTrue("isRunning");
                }
            }
            else
            {
                TriggeredByAttack = false;
            }

        }else
        {
            if (Vector3.Distance(transform.position, SpawnPoint) > 2.0f)
            {
                easyAnimator.SetBooleanTrue("isWalking");
                MoveTo(false, normalSpeedModifier, SpawnPoint);
            }
            else
            {
                easyAnimator.SetBooleanTrue("isIdling");
            }
            
        }
    }
    override public void SetDamage(int damageAmount, DamageType damageType)
    {
        if (!HasDoneAggro) HasDoneAggro = true;
        base.SetDamage(damageAmount, damageType);
    }
}
