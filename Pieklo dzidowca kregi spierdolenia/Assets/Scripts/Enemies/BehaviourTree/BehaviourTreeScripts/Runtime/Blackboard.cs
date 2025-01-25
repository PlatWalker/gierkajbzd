using System.Collections;
using System.Collections.Generic;
using jbzd.MainHero;
using UnityEngine;
using UnityEngine.AI;
using jbzd.Enemies;
using jbzd.Enemies.Obsolete;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {
        public DamageController damageController;
        public NavMeshAgent NavAgent;
        public EasyAnimatorController easyAnimator;
        public EnemyDataContainer EnemyData;
        public EnemySoundController soundController;
        public GameObject Prefab;
        public DamageController RHCollider;
        public DamageController LHCollider;
        public Vector3 moveToPosition;
        public Vector3 spawnPoint;
        public float waitBeforeSpreadDuration;
        public float dealDamageTimer = 1f;
        public float disappearTimer;
        public float spawnWanderRadius;
        public float distanceToMainChar;
        public float stopDistance;
        public float speed;
        public float wanderEveryXSeconds;
        public float multiUseTimer;
        public float maxSpreadDistance;
        public int currentHealth;
        public int framesCounter;
        public int tryCounter;
        public bool isDead;
        public bool startTimer;
        public bool isAggroing;
        public bool isAttacking;
        public bool isChasing;
        public bool isIdling;
        public bool isWandering;
        public bool deathAnimationDone;
        public bool playerIsVisible;
        public bool wanderTowardsSpawn;
        public bool updateLogicFrame;
        public bool generateNewPatrollingPoint;
        public bool GenerateNewDestination;
        public bool aggroCommenced;
        public bool moveEnemy;
        public bool moved;
        public bool isMother;
        public bool isInvincible;
        public float baseTime;
        public float squareRootMultiplayer;
        public float multiplayer;
        public float deviation;
        public float moveDuration;
        public float maxDimension;
        public PlayerManager _playerManager;
    }
}