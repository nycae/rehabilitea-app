using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Piano
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private uint        id;
        [SerializeField] private AudioClip   note;
        [SerializeField] private AudioSource source;

        public delegate void Played(uint tileID);
        public event Played  OnPlayed;

        public void Play()
        {
            source.clip = note;
            source.Play();
            OnPlayed?.Invoke(id);
        }
    }
}
