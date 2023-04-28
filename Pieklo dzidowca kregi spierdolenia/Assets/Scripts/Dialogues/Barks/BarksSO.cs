using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace jbzd.Dialogues.Barks
{
    [CreateAssetMenu(menuName = "Barks/BarksSO", fileName = "New Barks")]
    public class BarksSO : ScriptableObject
    {
        public List<string> barks = new();
        public List<BarksCondintionSO> startConditions;
        public List<BarksCondintionSO> endConditions;
        public bool showOnce = false;

        public bool IsReady(RunnerFactory factory, bool isStarting = true)
        {
            var conditions = isStarting ? startConditions : endConditions;

            var fulfilledNumber = 0;
            
            foreach (var condition in conditions)
            {
                var runner = factory.Create(condition);
                runner.Run();

                if (condition.isFulfilled) fulfilledNumber++;
                
            }

            return fulfilledNumber == conditions.Count;
        }
    }
}
