using UnityEngine;

namespace jbzd.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public delegate void EnemyDied();
        public abstract event EnemyDied OnDeath;
    }
}