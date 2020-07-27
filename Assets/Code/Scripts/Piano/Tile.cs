using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Piano
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private uint           id      = 0;
        [SerializeField] private AudioClip      note    = null;
        [SerializeField] private AudioSource    source  = null;

        public delegate void    Played(uint tileID);
        public event Played     OnPlayed;

        public void Play()
        {
            source.clip = note;
            source.Play();
            OnPlayed.Invoke(id);
        }
    }
}
