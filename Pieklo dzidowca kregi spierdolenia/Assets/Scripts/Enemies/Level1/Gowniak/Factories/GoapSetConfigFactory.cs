using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using jbzd.Enemies.Level1.Gowniak.Actions;
using jbzd.Enemies.Level1.Gowniak.Goals;
using jbzd.Enemies.Level1.Gowniak.Sensors;
using jbzd.Enemies.Level1.Gowniak.Targets;
using jbzd.Enemies.Level1.Gowniak.WorldKeys;
using UnityEngine;

namespace jbzd.Enemies.Level1.Gowniak.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        private DependencyInjector Injector;
        
        public override IGoapSetConfig Create()
        {
            Injector = GetComponent<DependencyInjector>();
            GoapSetBuilder builder = new("GowniakSet");

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(GoapSetBuilder builder)
        {
            builder.AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

            builder.AddGoal<KillPlayer>()
                .AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);
        }
        
        private void BuildActions(GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5)
                .SetInRange(10);

            builder.AddAction<MeleeAction>()
                .SetTarget<PlayerTarget>()
                .AddEffect<PlayerHealth>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.MeleeAttackCost)
                .SetInRange(Injector.AttackConfig.SensorRadius);
        }
        
        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();

            builder.AddTargetSensor<PlayerTargetSensor>()
                .SetTarget<PlayerTarget>();

            builder.AddWorldSensor<PlayerDistanceSensor>()
                .SetKey<PlayerDistance>();
        }
    }
}