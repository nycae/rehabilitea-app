using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RehabiliTEA
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[]    encouragementSounds    = null;
        [SerializeField] private AudioClip[]    rewardingSounds        = null;
        [SerializeField] private AudioClip      positiveEndgameSound    = null;
        [SerializeField] private AudioClip      negativeEndgameSound    = null;

        private void Awake()
        {
            Assert.IsNotNull(gameObject.GetComponent<AudioSource>());
        }

        private void PlayClip(AudioClip clip)
        {
            var audioSource     = gameObject.GetComponent<AudioSource>();
            audioSource.clip    = clip;
            audioSource.Play();
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
    }
}