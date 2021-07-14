using UnityEngine;

namespace jbzdy.Enemies
{
    public class SoundPlayer
    {
        AudioSource audioSource;
        public SoundPlayer(AudioSource _audioSource,AudioClip _audioClip)
        {
            if (_audioClip == null)
            {
                GameObject.Destroy(_audioSource);
                return;
            }
            audioSource = _audioSource;
            audioSource.clip = _audioClip;
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
        }
        public void Play()
        {
            if (audioSource == null) return;
            audioSource.Play();
        }
    }
}