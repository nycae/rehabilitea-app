using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Piano
{
    public class SongManager : MonoBehaviour
    {
        [System.Serializable] private class Song 
        { 
            public string       songName        = null;
            public uint[]       tileSequence    = null;
            public AudioClip    clip            = null;
        }

        [SerializeField] private Song[]         songs           = null;
        [SerializeField] private AudioSource    channel         = null;

        private Dictionary<string, AudioClip>   songClips       = new Dictionary<string, AudioClip>();
        private Dictionary<string, uint[]>      songSequences   = new Dictionary<string, uint[]>();
        private Queue<uint>                     targetSquence   = null;
        private bool                            isPlayingSong   = false;
        private string                          targetSong      = null;

        public delegate void                    SongStarted(string songName);
        public delegate void                    SongPlayed(string songName);
        public event SongStarted                OnSongBegin;
        public event SongPlayed                 OnSongEnd;

        void Start()
        {
            OnSongBegin += StopSong;
            OnSongEnd   += SendSongEndNotification;
            OnSongEnd   += StopPlayingSong;

            foreach (var tile in FindObjectsOfType<Tile>())
            {
                tile.OnPlayed += AttendTile;
            }

            foreach (var song in songs)
            {
                songClips.Add(song.songName, song.clip);
                songSequences.Add(song.songName, song.tileSequence);
            }

            songs = null;
        }

        void AttendTile(uint tileID)
        {
            if (isPlayingSong)
            {
                if (targetSquence.Count > 0)
                {   
                    if (targetSquence.Peek() == tileID)
                    {
                        targetSquence.Dequeue();
                        
                        if (targetSquence.Count <= 0)
                        {
                            isPlayingSong = false;

                            channel.clip = songClips[targetSong];
                            channel.Play();

                            OnSongEnd.Invoke(targetSong);
                        }
                    }
                }
            }
        }

        public void PlaySong(string songName)
        {
            isPlayingSong   = true;
            targetSong      = songName;
            targetSquence   = new Queue<uint>(songSequences[songName]);

            OnSongBegin.Invoke(songName);
        }

        public uint GetNextID()
        {
            return (isPlayingSong) ? targetSquence.Peek() : 0;
        }

        public uint[] GetSequence()
        {
            return (isPlayingSong) ? targetSquence.ToArray() : null;
        }

        public int GetRemainingNotes()
        {
            return (isPlayingSong) ? targetSquence.Count : 0;
        }

        void SendSongEndNotification(string songName)
        {
            RehabiliTEA.Profile.GetProfile().PostEvent("Song " + songName + " played");
        }

        void StopSong(string songName)
        {
            channel.Stop();
        }

        void StopPlayingSong(string songName)
        {
            isPlayingSong = true;
        }
    }
}