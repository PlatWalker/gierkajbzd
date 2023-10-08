using jbzd.Dialogues.Barks;
using UnityEngine;

namespace jbzd
{
    public class BarksOnTriggerEnter : MonoBehaviour
    {
        [field: SerializeField] private BarksTrigger barksTrigger;
        [field: SerializeField] private bool isStart;

        private void OnTriggerEnter(Collider other)
        {

            if (barksTrigger != null)
            {
                if (isStart)
                    barksTrigger.StartBarks();
                else
                    barksTrigger.StopBarks();
            }
        }
    }
}
