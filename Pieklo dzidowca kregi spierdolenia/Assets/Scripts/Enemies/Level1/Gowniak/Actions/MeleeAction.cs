using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using jbzd.Enemies.Level1.Gowniak.Config;
using UnityEngine;

namespace jbzd.Enemies.Level1.Gowniak.Actions
{
    public class MeleeAction : ActionBase<AttackData>, IInjectable
    {
        
        public static readonly int ATTACK = Animator.StringToHash("Attack");
        private AttackConfigSO AttackConfig;
        
        public override void Created() {}

        public override void Start(IMonoAgent agent, AttackData data)
        {
            data.Timer = AttackConfig.AttackDelay;
            data.DamageControllersManager.DamageDealed = false;
            data.DamageControllersManager.SetUp(AttackConfig.damage, AttackConfig.damageType, AttackConfig.criticalChance, AttackConfig.criticalMultiplier);

            if (data.Animator.parameters.Any(x => x.nameHash == ATTACK) == false)
                Debug.LogError("Blad w nazwie parametru ataku");
        }

        public override ActionRunState Perform(IMonoAgent agent, AttackData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            bool shouldAttack = data.Target != null &&
                                Vector3.Distance(data.Target.Position, agent.transform.position) <=
                                AttackConfig.MeleeAttackRadius;
            data.Animator.SetBool(ATTACK, shouldAttack);

            if (shouldAttack)
            {
                agent.transform.LookAt(data.Target.Position);
            }

            return data.Timer > 0 ? ActionRunState.Continue : ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, AttackData data)
        {
            data.Animator.SetBool(ATTACK, false);
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }
    }
}