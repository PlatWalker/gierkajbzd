using jbzd.MinorSystems.Interact;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    public class ChangeEmissionIfNotInteractable : MonoBehaviour
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        [SerializeField] private Color notInteractableEmissionColor;
        [SerializeField] private Color interactableEmissionColor;
        
        [SerializeField] private MeshRenderer meshRenderer;
        private Interaction _interaction;
        
        private void Awake()
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            _interaction = GetComponentInChildren<Interaction>();
        }
        
        private void Update()
        {
            meshRenderer.material.SetColor(EmissionColor,
                _interaction.IsInteractable ? interactableEmissionColor : notInteractableEmissionColor);
        }
    }
}
