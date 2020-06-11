using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Framework
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        public Dictionary<string, Environment.Sound> sounds;

        void Awake()
        {
            foreach (var sound in sounds)
            {
                sound.Value.audioSource         = gameObject.GetComponent<AudioSource>();
                sound.Value.audioSource.clip    = sound.Value.audioClip;
                sound.Value.audioSource.volume  = sound.Value.volume;
                sound.Value.audioSource.pitch   = sound.Value.pitch;
            }
        }

        void PlaySound(string key)
        {
            Environment.Sound sound;

            if (sounds.TryGetValue(key, out sound))
            {
                sound.audioSource.Play();
            }
        }
    }
}