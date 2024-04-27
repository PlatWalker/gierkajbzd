using CrashKonijn.Goap.Behaviours;
using jbzd.Enemies.Level1.Gowniak.Config;
using jbzd.Enemies.Level1.Gowniak.Goals;
using jbzd.Enemies.Sensors;
using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.Enemies.Level1.Gowniak.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GowniakBrain : EnemyBrain
    {
        [SerializeField] private PlayerSensor PlayerSensor;
        [SerializeField] private AttackConfigSO AttackConfig;
        [SerializeField] private HPConfigSO HPConfig;


        private AgentBehaviour AgentBehavior;
        private bool PlayerIsInRange;

        private void Awake()
        {
            AgentBehavior = GetComponent<AgentBehaviour>();
        }
        
        private void Start()
        {
            AgentBehavior.SetGoal<WanderGoal>(false);

            CurrentHealth = HPConfig.CurrentHealth;
            MaximumHealth = HPConfig.MaximumHealth;
            PlayerSensor.Collider.radius = AttackConfig.SensorRadius;
        }

        private void Update()
        {
            SetGoal();
            if(CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void SetGoal()
        {
            if (PlayerIsInRange)
            {
                AgentBehavior.SetGoal<KillPlayer>(true);
            }
            else
            {
                AgentBehavior.SetGoal<WanderGoal>(true);
            }
        }

        private void OnEnable()
        {
            PlayerSensor.OnPlayerEnter += PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit += PlayerSensorOnPlayerExit;
        }

        private void OnDisable()
        {
            PlayerSensor.OnPlayerEnter -= PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit -= PlayerSensorOnPlayerExit;
        }

        private void PlayerSensorOnPlayerExit(Vector3 lastKnownPosition)
        {
            PlayerIsInRange = false;
            AgentBehavior.SetGoal<WanderGoal>(true);
        }

        private void PlayerSensorOnPlayerEnter(Transform Player)
        {
            PlayerIsInRange = true;
            AgentBehavior.SetGoal<KillPlayer>(true);
        }


    }
}