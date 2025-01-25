using System;
using UnityEngine;

namespace jbzd.Enemies.Obsolete
{
    [Obsolete]
    public class SoundPlayer
    {
        AudioSource audioSource;
        public SoundPlayer(MonoBehaviour soundEmmitingObject,AudioClip _audioClip)
        {
            audioSource = soundEmmitingObject.gameObject.AddComponent<AudioSource>();
            audioSource.clip = _audioClip;
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
        }
        public void Play()
        {
            if (audioSource == null) return;
            audioSource.Play();
        }
        public void SetVolume(float _volume)
        {
            if (_volume > 1f) {
                audioSource.volume = 1f;
            }
            else if(_volume<0f) 
            {
                audioSource.volume = 0f;

            }
            else
            {
                audioSource.volume = _volume;
            }
        }

        public void SetSpatial(float _spatial)
        {
            if (_spatial > 1f)
            {
                audioSource.spatialBlend = 1f;
            }
            else if (_spatial < 0f)
            {
                audioSource.spatialBlend = 0f;

            }
            else
            {
                audioSource.spatialBlend = _spatial;
            }
        }
    }
}