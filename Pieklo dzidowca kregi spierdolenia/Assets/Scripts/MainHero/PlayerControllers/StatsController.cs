using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jbzd.Common.Interfaces;
using jbzd.UI;
using jbzd.UI.Hud;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace jbzd.MainHero.PlayerControllers
{
    [RequireComponent(typeof(Volume))]
    public class StatsController: MonoBehaviour, IPlayerController, IDamageable
    {
        private List<SkinnedMeshRenderer> _meshRenderers;
        private Volume _damagedVolume;
        private HudUIController _hudUIController;
        private UserInterfaceManager _userInterfaceManager;
        
        [SerializeField] private Material hitMaterial;
        [field:SerializeField] public bool IsInvincible { get; private set; }
        [SerializeField] private float invincibilityDurationSeconds;
        
        [field:SerializeField]
        public int MaximumHealth { get; set; }
        
        [field:SerializeField]
        public int CurrentHealth { get; set; }

        [field:SerializeField]
        private AudioSource AudioData { get; set; }
        [field:SerializeField]
        private float StartSoundFrom { get; set; }

        [Inject]
        public void Constructor(UserInterfaceManager userInterfaceManager)
        {
            _userInterfaceManager = userInterfaceManager;
        }
        
        public void Awake()
        {
            CurrentHealth = MaximumHealth;
            _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            _damagedVolume = GetComponentInChildren<Volume>();
            _hudUIController = _userInterfaceManager.GetUIController<HudUIController>();
            _hudUIController.Init(MaximumHealth);
            Debug.Assert(_damagedVolume, "Missing volume component in Player character");
        }

        public void SetDamage(int damageAmount, Vector3 damageOriginPoint)
        {
            if(IsInvincible) return;
            
            CurrentHealth -= damageAmount;
            _hudUIController.UpdateHealthBar(CurrentHealth);
            AudioData.time = StartSoundFrom;
            AudioData.Play();
            
            if (CurrentHealth <= 0)
            {
                Debug.LogError("You dead nigga");
            }
            
            StartCoroutine(BecomeHit());
            StartCoroutine(BecomeInvincible());
        }

        private IEnumerator BecomeHit()
        {
            var oldMaterials = _meshRenderers.Select(meshRenderer => (meshRenderer, meshRenderer.material)).ToList(); 
            _damagedVolume.profile.TryGet(out Vignette vignette);
            
            _meshRenderers.ForEach(meshRenderer => meshRenderer.material = hitMaterial);
            vignette.intensity.Override(0.25f);
            
            yield return new WaitForSeconds(invincibilityDurationSeconds);

            vignette.intensity.Override(0);
            _meshRenderers.ForEach(meshRenderer => meshRenderer.material = oldMaterials.First(tuple => tuple.meshRenderer == meshRenderer).material);
        }

        private IEnumerator BecomeInvincible()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(invincibilityDurationSeconds);
            IsInvincible = false;
        }
    }
}