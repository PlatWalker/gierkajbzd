using UnityEngine;

namespace jbzdy.Actions.Interaction
{
    public class InteractionZone : MonoBehaviour
    {
        [SerializeField]
        private bool isInRange;

        public bool IsInRange { get => isInRange; set => isInRange = value; }

        public delegate void PlaceReached(GameObject gameObject);
        public static event PlaceReached OnPlaceReach;

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                isInRange = true;
                OnPlaceReach?.Invoke(this.gameObject);
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
