using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {
        public EnemyDataContainer PokrzywaData;
        public DamageController damageController;
        public GameObject pokrzywaPrefab;
        public Vector3 moveToPosition;
        public float waitBeforeSpreadDuration = 10f;
        public float dealDamageTimer = 1f;
        public float disappearTimer;
        public bool isDead;
        public bool startTimer;
        public bool deathAnimationDone;
        public int currentHealth;
    }
}