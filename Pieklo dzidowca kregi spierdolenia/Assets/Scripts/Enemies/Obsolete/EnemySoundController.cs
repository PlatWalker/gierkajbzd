using System;
using UnityEngine;

namespace jbzd.Enemies.Obsolete
{
    [Obsolete]
    public class EnemySoundController : MonoBehaviour
    {
        private SoundPlayer attackSound;
        private SoundPlayer ambientSound;
        private SoundPlayer aggroSound;
        private SoundPlayer dyingSound;
        private SoundPlayer damagedSound;

        [SerializeField] protected float spatialBlendAttack = 1.0f;
        [SerializeField] protected float spatialBlendAmbient = 1.0f;
        [SerializeField] protected float spatialBlendAggro = 1.0f;
        [SerializeField] protected float spatialBlendDying = 1.0f;
        [SerializeField] protected float spatialBlendDamaged = 1.0f;

        [SerializeField] protected float volumeAttack = 1.0f;
        [SerializeField] protected float volumeAmbient = 1.0f;
        [SerializeField] protected float volumeAggro = 1.0f;
        [SerializeField] protected float volumeDying = 1.0f;
        [SerializeField] protected float volumeDamaged = 1.0f;


        protected EnemyDataContainer EnemyData { get; private set; }

        void Start()
        {
            EnemyController SoundEmmitingObject;
            if (!TryGetComponent<EnemyController>(out SoundEmmitingObject))
            {
                Debug.Log("Could not find Enemy Data to play sounds");
                Destroy(this);
            }
            EnemyData = SoundEmmitingObject.EnemyData;

            attackSound = new SoundPlayer(this, EnemyData.AttackAudioClip);
            ambientSound = new SoundPlayer(this, EnemyData.AmbientAudioClip);
            aggroSound = new SoundPlayer(this, EnemyData.AggroAudioClip);
            dyingSound = new SoundPlayer(this, EnemyData.DyingAudioClip);
            damagedSound = new SoundPlayer(this, EnemyData.DamagedAudioClip);
            
            attackSound.SetSpatial(spatialBlendAttack);
            attackSound.SetVolume(volumeAttack);
            ambientSound.SetSpatial(spatialBlendAmbient);
            ambientSound.SetVolume(volumeAmbient);
            aggroSound.SetSpatial(spatialBlendAggro);
            aggroSound.SetVolume(volumeAggro);
            dyingSound.SetSpatial(spatialBlendDying);
            dyingSound.SetVolume(volumeDying);
            damagedSound.SetSpatial(spatialBlendDamaged);
            damagedSound.SetVolume(volumeDamaged);

        }

        public void PlayAttack()
        {
            attackSound.Play();
        }
        public void PlayAggro()
        {
            aggroSound.Play();
        }
        public void PlayAmbient()
        {
            ambientSound.Play();
        }
        public void PlayDying()
        {
            dyingSound.Play();
        }
        public void PlayDamaged()
        {
            damagedSound.Play();
        }
    }
}