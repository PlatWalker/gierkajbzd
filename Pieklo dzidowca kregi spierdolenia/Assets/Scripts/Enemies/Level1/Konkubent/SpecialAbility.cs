using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/SpecialAbility")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "SpecialAbility", message: "[Agent] has ability ready", category: "Events", id: "52b1a632653e727f01c09746d3863133")]
public partial class SpecialAbility : EventChannelBase
{
    public delegate void SpecialAbilityEventHandler(GameObject Agent);
    public event SpecialAbilityEventHandler Event; 

    public void SendEventMessage(GameObject Agent)
    {
        Event?.Invoke(Agent);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AgentBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Agent = AgentBlackboardVariable != null ? AgentBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Agent);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        SpecialAbilityEventHandler del = (Agent) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Agent;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as SpecialAbilityEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as SpecialAbilityEventHandler;
    }
}

