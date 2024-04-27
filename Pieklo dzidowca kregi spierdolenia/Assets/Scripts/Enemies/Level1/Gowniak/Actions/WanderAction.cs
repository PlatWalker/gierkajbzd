using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace jbzd.Enemies.Level1.Gowniak.Actions
{
    public class WanderAction : ActionBase<CommonData>
    {
        
        public override void Created() {}

        public override void Start(IMonoAgent agent, CommonData data)
        {
            data.Timer = Random.Range(2,4);
        }

        public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
        {
            data.Timer -= context.DeltaTime;

            if (data.Timer > 0)
            {
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, CommonData data) {}
        
    }
}