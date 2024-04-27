using UnityEngine;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using jbzd.Enemies.Level1.Gowniak.Config;
using jbzd.MainHero;

namespace jbzd.Enemies.Level1.Gowniak.Sensors
{
    public class PlayerDistanceSensor : LocalWorldSensorBase, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private Collider[] Colliders = new Collider[1];
        
        public override void Created() {}
        public override void Update() {}


        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            if(AttackConfig == null){
                Debug.LogError("AttackConfig is null");
                return int.MaxValue;
            }
            if (Physics.OverlapSphereNonAlloc(
                    agent.transform.position,
                    AttackConfig.SensorRadius,
                    Colliders,
                    AttackConfig.AttackableLayerMask
                ) > 0 && Colliders[0].TryGetComponent(out PlayerManager player))
            {
                return new SenseValue(
                    Mathf.CeilToInt(Vector3.Distance(agent.transform.position, player.transform.position))
                );
            }

            return int.MaxValue;
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}