using System.Linq;
using jbzd.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace jbzd.MainHero.LegacyHeroThings
{
    public class WeaponController : MonoBehaviour
    {
        private int damageAmount = 10;
        private DamageType typeOfDamage = DamageType.CloseCombat;

        private float CritChance = 0.0f;
        private float CritMultiplier = 1.0f;

        private Animator _characterAnimator;

        private PlayerManager _playerController;

        [Inject]
        public void Construct(PlayerManager playerController)
        {
            _playerController = playerController;
            _characterAnimator = _playerController.gameObject.GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter(Collider collidedObject)
        {
            collidedObject.gameObject.TryGetComponent<IDamageable>(out var hitObjectScript);

            if (hitObjectScript != null &&
                _characterAnimator.GetBool(PlayerStringAnimParam.AttackInProgressParam) &&
                _playerController.ListOfEnemiesColliders.All(x => x != collidedObject))
            {
                hitObjectScript.SetDamage(damageAmount, typeOfDamage, CritMultiplier, CritChance);
                _playerController.ListOfEnemiesColliders.Add(collidedObject);
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
}
