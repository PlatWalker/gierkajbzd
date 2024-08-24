using jbzd.MinorSystems.Interact;
using UnityEngine;

namespace jbzd.QuestCreationScripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChangeColorIfNotInteractable: MonoBehaviour
    {
        [SerializeField] private Material notInteractableMaterial;
        [SerializeField] private Material interactableMaterial;
        
        private MeshRenderer _meshRenderer;
        private Interaction _interaction;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _interaction = GetComponentInChildren<Interaction>();
        }
        
        private void Update()
        {
            _meshRenderer.material = _interaction.IsInteractable is false ? notInteractableMaterial : interactableMaterial;
        }
    }
}