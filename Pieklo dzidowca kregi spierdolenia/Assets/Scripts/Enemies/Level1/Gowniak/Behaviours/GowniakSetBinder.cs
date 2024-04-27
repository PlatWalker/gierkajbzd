using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace jbzd.Enemies.Level1.Gowniak.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GowniakSetBinder : MonoBehaviour
    {
        [SerializeField] private GoapRunnerBehaviour GoapRunner;

        private void Awake()
        {
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            agent.GoapSet = GoapRunner.GetGoapSet("GowniakSet");
        }
    }
}