using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StartRechargingAbility")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StartRechargingAbility", message: "[Agent] can start recharging [ability]", category: "Events", id: "ed17d32caf50ce85ac6943fcb1e8fb8c")]
public partial class StartRechargingAbility : EventChannelBase
{
    public delegate void StartRechargingAbilityEventHandler(GameObject Agent, string ability);
    public event StartRechargingAbilityEventHandler Event; 

    public void SendEventMessage(GameObject Agent, string ability)
    {
        Event?.Invoke(Agent, ability);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AgentBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Agent = AgentBlackboardVariable != null ? AgentBlackboardVariable.Value : default(GameObject);

        BlackboardVariable<string> abilityBlackboardVariable = messageData[1] as BlackboardVariable<string>;
        var ability = abilityBlackboardVariable != null ? abilityBlackboardVariable.Value : default(string);

        Event?.Invoke(Agent, ability);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        StartRechargingAbilityEventHandler del = (Agent, ability) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Agent;

            BlackboardVariable<string> var1 = vars[1] as BlackboardVariable<string>;
            if(var1 != null)
                var1.Value = ability;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as StartRechargingAbilityEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as StartRechargingAbilityEventHandler;
    }
}

