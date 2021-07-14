using UnityEngine;

namespace jbzdy.Enemies
{
    public class EnemySoundController
    {
        private SoundPlayer attackSound;
        private SoundPlayer ambientSound;
        private SoundPlayer aggroSound;
        private SoundPlayer dyingSound;
        private SoundPlayer damagedSound;

        protected MonoBehaviour SoundEmmitingObject { get; private set; }
        protected EnemyDataContainer EnemyData { get; private set; }

        public EnemySoundController(MonoBehaviour soundEmmitingObject,EnemyDataContainer data)
        {
            EnemyData = data;
            SoundEmmitingObject = soundEmmitingObject;
        }

        public virtual void Initialize()
        {
            attackSound = new SoundPlayer(SoundEmmitingObject.gameObject.AddComponent<AudioSource>(), EnemyData.AttackAudioClip);
            ambientSound = new SoundPlayer(SoundEmmitingObject.gameObject.AddComponent<AudioSource>(), EnemyData.AmbientAudioClip);
            aggroSound = new SoundPlayer(SoundEmmitingObject.gameObject.AddComponent<AudioSource>(), EnemyData.AggroAudioClip);
            dyingSound = new SoundPlayer(SoundEmmitingObject.gameObject.AddComponent<AudioSource>(), EnemyData.DyingAudioClip);
            damagedSound = new SoundPlayer(SoundEmmitingObject.gameObject.AddComponent<AudioSource>(), EnemyData.DamagedAudioClip);

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