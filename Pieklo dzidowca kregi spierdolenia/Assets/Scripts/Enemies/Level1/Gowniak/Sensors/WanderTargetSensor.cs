using UnityEngine;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using jbzd.Enemies.Level1.Gowniak.Config;

namespace jbzd.Enemies.Level1.Gowniak.Sensors
{
    public class WanderTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private WanderConfigSO WanderConfig;
        public override void Created() {}

        public override void Update() {}

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            Vector3 position = GetRandomPosition(agent);
            
            return new PositionTarget(position);
        }

        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            int count = 0;

            while (count < 5)
            {
                Vector2 random = Random.insideUnitCircle * WanderConfig.WanderRadius;
                Vector3 position = agent.transform.position + new Vector3(
                    random.x,
                    0,
                    random.y
                );
                if (UnityEngine.AI.NavMesh.SamplePosition(position, out UnityEngine.AI.NavMeshHit hit, 1, UnityEngine.AI.NavMesh.AllAreas))
                {
                    return hit.position;
                }

                count++;
            }

            return agent.transform.position;
        }

        public void Inject(DependencyInjector injector)
        {
            WanderConfig = injector.WanderConfig;
        }
    }
}