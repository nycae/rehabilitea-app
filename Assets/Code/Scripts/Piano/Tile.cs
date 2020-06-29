using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Piano
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private AudioClip      note    = null;
        [SerializeField] private AudioSource    source  = null;

        public void Play()
        {
            source.clip = note;
            source.Play();
        }
    }
}
