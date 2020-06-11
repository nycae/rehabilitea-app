using UnityEngine.Audio;
using UnityEngine;


namespace Environment
{
    [System.Serializable]
    public class Sound
    {

        [Range(0f, 1f)]
        public float        volume;

        [Range(.1f, 3f)]
        public float        pitch;

        [HideInInspector]
        public AudioSource  audioSource;

        [SerializeField]
        public AudioClip    audioClip;
    }
}
