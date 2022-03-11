using System.Collections.Generic;
using System.Linq;
using jbzdy.Player;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private int damageAmount = 10;
    private DamageType typeOfDamage = DamageType.CloseCombat;

    private float CritChance = 0.0f;
    private float CritMultiplier = 1.0f;

    private Animator characterAnimator;
    private PlayerController playerController;

    public void Start()
    {
        characterAnimator = GameManager.Instance.PlayerObject.GetComponentInChildren<Animator>();
        playerController = GameManager.Instance.PlayerObject.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider collidedObject)
    {
        collidedObject.gameObject.TryGetComponent<IDamageable>(out var hitObjectScript);

        if (hitObjectScript != null &&
            characterAnimator.GetBool(StringAnimatorParameters.AttackInProgressParam) &&
            playerController.playerAttackController.listOfEnemiesColliders.All(x => x != collidedObject))
        {
            hitObjectScript.SetDamage(damageAmount, typeOfDamage, CritMultiplier, CritChance);
            playerController.playerAttackController.listOfEnemiesColliders.Add(collidedObject);
        }
        /* Debug do refactoru
        else if (hitObjectScript != null)
        {
            Debug.Log("Why no attac?:" +
                      "  || isAttacking? => " 
                      + characterAnimator.GetBool(StringAnimatorParameters.AttackInProgressParam) +
                      " || isOnList? => " +
                      playerController.playerAttackController.listOfEnemiesColliders.Any(x => x == collidedObject));
        }
        */

    }
}
