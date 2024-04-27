using UnityEngine;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using jbzd.MainHero;
namespace jbzd.Enemies.Level1.Gowniak.Config
{
    [CreateAssetMenu(menuName = "Gowniak/Attack Config", fileName = "Gowniak Attack Config", order = 1)]
    public class AttackConfigSO : ScriptableObject
    {
        public float SensorRadius = 5f;
        public float MeleeAttackRadius = 1f;
        public int MeleeAttackCost = 1;
        public float AttackDelay = 1f;


        public int damage=0;
        public float criticalMultiplier = 0;
        public float criticalChance = 0;
        public DamageType damageType=DamageType.CloseCombat;
        public LayerMask AttackableLayerMask; 
    }
}