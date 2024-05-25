using System.Collections.Generic;
using System.Linq;
using jbzd.MainHero;
using UnityEngine;
using Zenject;

namespace jbzd.Scenes.PlayerBehindObstaclesView.V2
{
    public class CameraFaderController : MonoBehaviour
    {
        private float maxFadeDistance = 10f; // Maximum distance for fading effect
        private Transform player; // Reference to the player's transform
        private List<ObjectFader> fadedObjects = new List<ObjectFader>(); // List to store faded objects
        private PlayerManager _playerController;
            
        [Inject]
        public void Construct(PlayerManager playerController)
        {
            _playerController = playerController;
        }
    
        void Start()
        {
            player = _playerController.transform;
        }

        void Update()
        {

            RaycastHit[] hits = Physics.RaycastAll(transform.position, (player.position - transform.position).normalized, maxFadeDistance);
            foreach (RaycastHit hit in hits)
            {
                // Check if the hit object has ObjectFaderComponent attached
                ObjectFader fader = hit.collider.GetComponent<ObjectFader>();
                if (fader != null)
                {
                    ObjectFader objectFader = fader.GetComponent<ObjectFader>();
                    if (objectFader != null && !fadedObjects.Contains(objectFader))
                    {
                        Debug.Log("Fading in object" + objectFader.gameObject.name);
                        fadedObjects.Add(objectFader);
                        objectFader.StartFadeOut();
                    }
                }
            }
            List<ObjectFader> objectsToRemove = new List<ObjectFader>();

            foreach (ObjectFader fader in fadedObjects)
            {
                if (!hits.Any(hit => hit.collider.gameObject == fader.gameObject))
                {
                    Debug.Log("Fader object hit: " + fader.gameObject.name);
                    fader.StartFadeIn();
                    objectsToRemove.Add(fader);
                }
            }

            foreach (ObjectFader fader in objectsToRemove)
            {
                fadedObjects.Remove(fader);
            }
            
        }

        // Method to add an object to the faded objects list
        public void AddToFadedObjects(ObjectFader fader)
        {
            fadedObjects.Add(fader);
        }
    }
}
