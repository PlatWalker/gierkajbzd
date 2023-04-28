using System.Collections.Generic;
using System.Linq;
using jbzd.Common.RunnerThing;
using UnityEngine;
using Zenject;

namespace jbzd.Dialogues.Barks
{
    public class BarksTrigger : MonoBehaviour
    {
        [field: SerializeField] private List<Barks> actors = new();

        public void StartBarks()
        {
            foreach (var actor in actors)
            {
                actor.CheckBarks(this);
            }
        }

        public void StopBarks()
        {
            foreach (var actor in actors)
            {
                actor.CheckBarks(this, false);
            }
        }
        
    }
}
