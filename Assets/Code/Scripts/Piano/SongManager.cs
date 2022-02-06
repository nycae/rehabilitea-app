using System;
using System.Collections.Generic;
using UnityEngine;

namespace Piano
{
    public class SongManager : MonoBehaviour
    {
        [Serializable] private class Song 
        {
            public string    songName;
            public uint[]    tileSequence;
            public AudioClip clip;
        }

        [SerializeField] private Song[]      songs;
        [SerializeField] private AudioSource channel;

        private readonly Dictionary<string, AudioClip> songClips     = new Dictionary<string, AudioClip>();
        private readonly Dictionary<string, uint[]>    songSequences = new Dictionary<string, uint[]>();
        private Queue<uint>                            targetSequence;
        private bool                                   isPlayingSong;
        private string                                 targetSong;

        public delegate void     SongStarted(string songName);
        public delegate void     SongPlayed(string songName);
        public event SongStarted OnSongBegin;
        public event SongPlayed  OnSongEnd;

        private void Start()
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

        private void AttendTile(uint tileID)
        {
            if (!isPlayingSong) return;
            if (targetSequence.Count <= 0) return;
            if (targetSequence.Peek() != tileID) return;
            targetSequence.Dequeue();

            if (targetSequence.Count > 0) return;
            isPlayingSong = false;

            channel.clip = songClips[targetSong];
            channel.Play();

            OnSongEnd?.Invoke(targetSong);
        }

        public void PlaySong(string songName)
        {
            isPlayingSong   = true;
            targetSong      = songName;
            targetSequence   = new Queue<uint>(songSequences[songName]);

            OnSongBegin?.Invoke(songName);
        }

        public IEnumerable<uint> GetSequence()
        {
            return isPlayingSong ? targetSequence.ToArray() : null;
        }

        public int GetRemainingNotes()
        {
            return isPlayingSong ? targetSequence.Count : 0;
        }

        private static void SendSongEndNotification(string songName)
        {
            RehabiliTEA.Profile.GetProfile().PostEvent("Song " + songName + " played");
        }

        private void StopSong(string songName)
        {
            channel.Stop();
        }

        private void StopPlayingSong(string songName)
        {
            isPlayingSong = true;
        }
    }
}