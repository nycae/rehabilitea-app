using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RehabiliTEA
{
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        public struct SoundPair
        {
            public string       key;
            public AudioClip    value;
        }

        [SerializeField] private SoundPair[]    otherSounds             = null;
        [SerializeField] private AudioClip[]    encouragementSounds     = null;
        [SerializeField] private AudioClip[]    rewardingSounds         = null;
        [SerializeField] private AudioClip      positiveEndgameSound    = null;
        [SerializeField] private AudioClip      negativeEndgameSound    = null;
        [SerializeField] private AudioSource    ambientAudioSource      = null;
        [SerializeField] private AudioSource    narratorAudioSource     = null;
        private Dictionary<string, AudioClip>   environmentSounds       = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            foreach (var pair in otherSounds)
            {
                environmentSounds.Add(pair.key, pair.value);
            }

            otherSounds = null;

            Assert.IsNotNull(ambientAudioSource);
            Assert.IsNotNull(narratorAudioSource);
        }

        private void PlayClip(AudioClip clip)
        {
            narratorAudioSource.clip = clip;
            narratorAudioSource.Play();
        }

        private void PlayEnvironment(AudioClip clip)
        {
            ambientAudioSource.clip = clip;
            ambientAudioSource.Play();
        }

        public void PlayRandomRewardingSound()
        {
            PlayClip(rewardingSounds[Random.Range(0, rewardingSounds.Length - 1)]);
        }

        public void PlayRandomEncouragementSound()
        {
            PlayClip(encouragementSounds[Random.Range(0, encouragementSounds.Length - 1)]);
        }

        public void PlayPositiveEndgameSound()
        {
            PlayClip(positiveEndgameSound);
        }

        public void PlayNegativeEndgameSound()
        {
            PlayClip(negativeEndgameSound);
        }

        public void PlaySoundAsAmbient(AudioClip clip)
        {
            PlayClip(clip);
        }

        public void PlayEnvironmentSound(string key)
        {
            PlayEnvironment(environmentSounds[key]);
        }
    }
}