using jbzd.Common.Interfaces;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    public class AnimationOnInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator animatorToPlay;

        public void OnInteract()
        {
            animatorToPlay.enabled = true;
        }
    }
}
