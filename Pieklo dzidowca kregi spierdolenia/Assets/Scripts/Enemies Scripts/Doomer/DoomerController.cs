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

    private bool hasDoneAggro;

    private bool hasDoneSpecialAttack;
    
    private float normalSpeedModifier = 1f;



    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        easyAnimator = new EasyAnimatorController(GetComponent<Animator>(), new string[] {"hasFinishedAggro","hasFinishedFirstAttack","shouldUseSecondAttack"});
        hasDoneSpecialAttack = false;
        hasDoneAggro = false;
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

        if (CurrentHealth <= 0)
        {
            Alive = false;
            DisappearTimer += Time.deltaTime;
            if (DisappearTimer >= DisappearAfter) Destroy(this.gameObject);
            easyAnimator.SetBooleanDirectly("isDying", true);
        }

        if (turnOffAI || !Alive ) return;


        easyAnimator.ResetAllBooleans();

        float distanceToMainChar = Vector3.Distance(transform.position, MainCharacterTransform.position);
       
        hasDoneAggro = easyAnimator.GetBoolean("hasFinishedAggro");
        hasDoneSpecialAttack = easyAnimator.GetBoolean("hasFinishedFirstAttack");

        if (distanceToMainChar < AggroRadius && distanceToMainChar > AttackRadius && PlayerVisible())// checking if main char is visible for enemy and should not attack
        {

            easyAnimator.SetBooleanDirectly("isSeeingPlayer",true);
            MoveTo(false, 0.0f, MainCharacterTransform.position);

            if (hasDoneAggro)
            {

                if (distanceToMainChar <= firstAttackRadius && !hasDoneSpecialAttack) //checking if should start special attack
                {

                    easyAnimator.SetBooleanDirectly("isPlayerReached",true);
                    easyAnimator.SetBooleanDirectly("isSeeingPlayer", true);//thanks to this animator know when start animation of jump
                    
                    if (jumpDirection == Vector3.zero) 
                    {
                        jumpDirection = MainCharacterTransform.position; //saving jump direction so enemy cannot change direction in air
                            
                    }

                    if (Vector3.Distance(jumpDirection, gameObject.transform.position) > AttackRadius)
                    {
                        MoveTo(false, jumpSpeed, jumpDirection);
                    }
                    else
                    {
                        easyAnimator.SetBooleanDirectly("hasFinishedFirstAttack",true);
                    }
                }
                else if(hasDoneSpecialAttack)
                {
                    //if attack has been made just move in normal speed to player
                    MoveTo(false, normalSpeedModifier, MainCharacterTransform.position);
                }
                else
                {
                    //when attack has been not made and cannot be made yet move to player in charge
                    MoveTo(false, movementRushSpeedModifier, MainCharacterTransform.position);
                }

            }            
        }
        else if (distanceToMainChar <= AttackRadius && PlayerVisible())
        {
            if (distanceToMainChar > 0.3)
            {
                MoveTo(false, 0.0f, MainCharacterTransform.position);
            }
            easyAnimator.SetBooleanDirectly("isPlayerReached", true);
            easyAnimator.SetBooleanDirectly("isPlayerReached", true);
            //tutaj zadawanie obrażeń - collider i te sprawy
            //dodać tutaj sprawdzenie czy zakończono atak specjalnt i jesli tak to normalne obrażenia a jak nie to dodatkowe obrażenia
        }
        else
        {
            if (Vector3.Distance(transform.position, SpawnPoint) > 2.0f)
            {
                easyAnimator.SetBooleanDirectly("shouldReturnToSpawn", true);
                MoveTo(false, normalSpeedModifier, SpawnPoint);
            }
            else
            { 
                easyAnimator.ResetAllBooleans();
            }
        }
    }
}
