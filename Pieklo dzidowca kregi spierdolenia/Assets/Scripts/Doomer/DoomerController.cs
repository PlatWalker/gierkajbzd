///<summary>
///Created By Kumdzio
///</summary>

using UnityEngine;

public class DoomerController : MonoBehaviour, IMove, IFight
{
    [SerializeField] private float _movementSpeed = 0.034f;
    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
        private set
        {
            _movementSpeed = value;
        }
    }

    [SerializeField] private float _rotationSpeed = 0.15f;
    public float RotationSpeed
    {
        get
        {
            return _rotationSpeed;
        }
        private set
        {
            _rotationSpeed = value;
        }
    }

    [SerializeField]private Transform _mainCharacterTransform;
    public Transform MainCharacterTransform
    {
        get
        {
            return _mainCharacterTransform;
        }
        
    }
    
    [SerializeField] private float _aggroRadius = 10.0f;
    public float AggroRadius
    {
        get
        {
            return _aggroRadius;
        }
        private set
        {
            _aggroRadius = value;
        }
    }

    public float AttackRadius { get; private set; } = 1.0f;

    [SerializeField] private float movementRushSpeed=1.05f;

    [SerializeField] private float jumpSpeed = 1.08f;

    private float firstAttackRadius = 4.0f;

    private Vector3 jumpDirection;

    private Vector3 spawnPoint;

    private Animator doomerAnimator;

    private bool hasDoneAggro;

    private bool hasDoneSpecialAttack;

    private bool isUsingChargedAttack;

    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        private set
        {
            _maxHealth = value;
        }
    }

    public int CurrentHealth { get; private set; }
    


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
        doomerAnimator = GetComponent<Animator>();
        hasDoneSpecialAttack = false;
        hasDoneAggro = false;
        isUsingChargedAttack = false;
        CurrentHealth = _maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Debug.Log(doomerAnimator.parameters);
        foreach (AnimatorControllerParameter parametr in doomerAnimator.parameters)
        {
            Debug.Log("AnimatorController name:" + doomerAnimator.name);
            Debug.Log("Ilość parametrów:" + doomerAnimator.parameters.Length);
            Debug.Log("type: "+parametr.type);
            Debug.Log(parametr.type == AnimatorControllerParameterType.Bool);
            Debug.Log("getType(): "+parametr.GetType());
            Debug.Log("defaultBool: "+parametr.defaultBool);
            Debug.Log("name: "+parametr.name);
            Debug.Log("nameHash: "+parametr.nameHash);
            Debug.Log("toString(): "+parametr.ToString());
        }
        */

        float distanceToMainChar = Vector3.Distance(gameObject.transform.position, _mainCharacterTransform.position);
        hasDoneAggro = doomerAnimator.GetBool("hasFinishedAggro");
        hasDoneSpecialAttack = doomerAnimator.GetBool("hasFinishedFirstAttack");
        if (hasDoneSpecialAttack)
        {
            isUsingChargedAttack = false;
        }

        if (distanceToMainChar < _aggroRadius && distanceToMainChar > AttackRadius)// checking if main char is visible for enemy and should not attack
        {
            doomerAnimator.SetBool("isSeeingPlayer", true);
            doomerAnimator.SetBool("shouldReturnToSpawn", false);
            doomerAnimator.SetBool("isPlayerReached", false);
            UpdateEnemyRotation(_mainCharacterTransform.position);
            if (hasDoneAggro)
            {
                if (distanceToMainChar <= firstAttackRadius && !hasDoneSpecialAttack) //checking if should start special attack
                {
                    doomerAnimator.SetBool("isPlayerReached", true);    //thanks to this animator know when start animation of jump
                    isUsingChargedAttack = true;
                    if (jumpDirection == new Vector3(0.0f,0.0f,0.0f)) 
                    {
                        jumpDirection = _mainCharacterTransform.position; //saving jump direction so enemy cannot change direction in air
                            
                    }
                    if (Vector3.Distance(jumpDirection, gameObject.transform.position) > 0.5f)
                    {
                        //any direction given here to UpdateEnemyPostion will be ignored
                        UpdateEnemyPosition(jumpDirection, jumpSpeed);
                    }
                }
                else if(hasDoneSpecialAttack)
                {
                    //if attack has been made just move in normal speed to player
                    UpdateEnemyPosition(new Vector3(_mainCharacterTransform.position.x - gameObject.transform.position.x, 0, _mainCharacterTransform.position.z - gameObject.transform.position.z));
                }
                else
                { 
                    //when attack has been not made and cannot be made yet move to player inc harge
                    UpdateEnemyPosition(new Vector3(_mainCharacterTransform.position.x - gameObject.transform.position.x, 0, _mainCharacterTransform.position.z - gameObject.transform.position.z),movementRushSpeed);
                }

            }            
        }
        else if (distanceToMainChar <= AttackRadius)
        {
            if (distanceToMainChar > 0.3)
            {
                UpdateEnemyRotation(MainCharacterTransform.position);
            }
            doomerAnimator.SetBool("isSeeingPlayer", true);
            doomerAnimator.SetBool("shouldReturnToSpawn", false);
            doomerAnimator.SetBool("isPlayerReached", true);
            //tutaj zadawanie obrażeń collider i te sprawy
            //dodać tutaj sprawdzenie czy zakończono atak specjalnt i jesli tak to normalne obrażenia a jak nie to dodatkowe obrażenia
        }
        else
        {
            doomerAnimator.SetBool("isSeeingPlayer", false);
            doomerAnimator.SetBool("isPlayerReached", false);
            if (Vector3.Distance(gameObject.transform.position, spawnPoint) > 2.0f)
            {
                doomerAnimator.SetBool("shouldReturnToSpawn", true);
                UpdateEnemyRotation(new Vector3(spawnPoint.x, 0, spawnPoint.z));
                UpdateEnemyPosition(new Vector3(spawnPoint.x - gameObject.transform.position.x, 0, spawnPoint.z - gameObject.transform.position.z));
            }
            else
            { 
                doomerAnimator.SetBool("shouldReturnToSpawn", false);
            }
        }
    }

    void UpdateEnemyPosition(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        this.UpdateEnemyPositionNormalized(direction);
    }

    void UpdateEnemyPosition(Vector3 direction, float speedModifier)
    {
        if (isUsingChargedAttack)
        {
            direction = new Vector3(
                jumpDirection.x - gameObject.transform.position.x,
                jumpDirection.y - gameObject.transform.position.y,
                jumpDirection.z - gameObject.transform.position.z
                );
        }
        direction = Vector3.Normalize(direction);

        direction *= speedModifier;
        this.UpdateEnemyPositionNormalized(direction);
    }

    void UpdateEnemyPositionNormalized(Vector3 normalizedDirection)
    {
        gameObject.transform.position += normalizedDirection * MovementSpeed;
    }



    void UpdateEnemyRotation(Vector3 direction)
    {
        if (isUsingChargedAttack)
        {
            direction.x = jumpDirection.x;
            direction.z = jumpDirection.z;
        }
        gameObject.transform.Rotate(0.0f, -90.0f, 0.0f); //reApply Bug of gizmos
        Vector3 targetDirection = direction - gameObject.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, _rotationSpeed, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
        gameObject.transform.Rotate(0.0f, 90.0f, 0.0f); // removing Bug of gizmos

    }

    public float GetHealthPercentage()
    {
        return (float)CurrentHealth / (float)_maxHealth;
    }

    //for now damage types are ignored
    public void SetDamage(int damageAmount, DamageType damageType)
    {
        CurrentHealth -= damageAmount;
    }
    
    public void SetDamage(int damageAmount, DamageType damageType, int criticalMultiplier, float criticalChance)
    {
        if (Random.Range(0.0f, 1.0f) <= criticalChance)
        {
            damageAmount *= criticalMultiplier;
        }
        this.SetDamage(damageAmount, damageType);
    }

}
