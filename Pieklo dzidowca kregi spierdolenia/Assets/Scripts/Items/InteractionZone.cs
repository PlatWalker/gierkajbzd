using UnityEngine;

namespace jbzdy.Actions.Interaction
{
    public class InteractionZone : MonoBehaviour
    {
        private bool isInRange;

        public bool IsInRange { get => isInRange; set => isInRange = value; }

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                isInRange = true;
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = false;
            }
        }
    }
}
