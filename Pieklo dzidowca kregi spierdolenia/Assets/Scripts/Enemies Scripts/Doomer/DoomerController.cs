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

   
    private Vector3 jumpDirection;

    private bool hasDoneAggro;

    private bool hasDoneSpecialAttack;



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

        if (turnOffAI) return;
        transform.Rotate(0.0f, -90.0f, 0.0f); //reApply Bug of gizmos have to be removed vefore endig this method
        easyAnimator.ResetAllBooleans();

        float distanceToMainChar = Vector3.Distance(transform.position, MainCharacterTransform.position);
       
        hasDoneAggro = easyAnimator.GetBoolean("hasFinishedAggro");
        hasDoneSpecialAttack = easyAnimator.GetBoolean("hasFinishedFirstAttack");

        if (distanceToMainChar < AggroRadius && distanceToMainChar > AttackRadius)// checking if main char is visible for enemy and should not attack
        {

            easyAnimator.SetBooleanDirectly("isSeeingPlayer",true);
            Move(false, 0.0f, MainCharacterTransform.position);

            if (hasDoneAggro)
            {

                if (distanceToMainChar <= firstAttackRadius && !hasDoneSpecialAttack) //checking if should start special attack
                {

                    easyAnimator.SetBooleanDirectly("isPlayerReached",true);
                    easyAnimator.SetBooleanDirectly("isSeeingPlayer", true);//thanks to this animator know when start animation of jump
                    
                    if (jumpDirection == new Vector3(0.0f,0.0f,0.0f)) 
                    {
                        jumpDirection = MainCharacterTransform.position; //saving jump direction so enemy cannot change direction in air
                            
                    }

                    if (Vector3.Distance(jumpDirection, gameObject.transform.position) > 0.5f)
                    {
                        //any direction given here to UpdateEnemyPostion will be ignored
                        Move(false, jumpSpeed, jumpDirection);
                    }
                }
                else if(hasDoneSpecialAttack)
                {
                    //if attack has been made just move in normal speed to player
                    Move(false, 1f, MainCharacterTransform.position);
                }
                else
                {
                    //when attack has been not made and cannot be made yet move to player in charge
                    Move(false, movementRushSpeedModifier, MainCharacterTransform.position);
                }

            }            
        }
        else if (distanceToMainChar <= AttackRadius)
        {
            if (distanceToMainChar > 0.3)
            {
                Move(false, 0.0f, MainCharacterTransform.position);
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
                Move(false, 1f, SpawnPoint);
            }
            else
            { 
                easyAnimator.ResetAllBooleans();
            }
        }
        transform.Rotate(0.0f, 90.0f, 0.0f); // removing Bug of gizmos - have to be reapplied before calculations
    }
}
