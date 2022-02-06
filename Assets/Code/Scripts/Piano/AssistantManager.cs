using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Piano
{
    public class AssistantManager : MonoBehaviour
    {
        [Serializable] private class NoteLocation
        {
            public uint    id;
            public Vector3 location;
        }

        [SerializeField] private Sprite[]       spriteNumber;
        [SerializeField] private NoteLocation[] noteLocationBuilder;
        [SerializeField] private SongManager    songManager;
        [SerializeField] private int            maxHints;

        private          GameObject[]              hints;
        private readonly Dictionary<uint, Vector3> noteLocations = new Dictionary<uint, Vector3>();
        private readonly Vector3                   numberScale   = new Vector3(0.25f, 0.25f, 1f);

        private void Start()
        {
            songManager.OnSongEnd   += AttendSongEnd;
            songManager.OnSongBegin += AttendSongBegin;

            foreach (var tile in FindObjectsOfType<Tile>())
            {
                tile.OnPlayed += AttendTilePlayed;
            }

            foreach (var noteLocation in noteLocationBuilder)
            {
                noteLocations.Add(noteLocation.id, noteLocation.location);
            }

            noteLocationBuilder = null;
        }

        private void SpawnHints()
        {
            var hintCount    = Math.Min(maxHints, songManager.GetRemainingNotes());
            var songSequence = songManager.GetSequence().Distinct().Take(hintCount).ToArray();
            
            hints = new GameObject[maxHints];

            for (var i = 0; i < Math.Min(maxHints, songSequence.Length); i++)
            {
                hints[i] = new GameObject 
                {
                    transform =
                        {position = noteLocations[songSequence[i]], localScale = numberScale}
                };

                if (hints[i].AddComponent(typeof(SpriteRenderer)) is SpriteRenderer spriteRendererBuffer) 
                    spriteRendererBuffer.sprite = spriteNumber[i];
            }
        }

        private void DeleteHints()
        {
            if (hints == null) return;
            
            foreach (var hint in hints)
            {
                Destroy(hint);
            }
        }

        private void AttendSongBegin(string songName)
        {
            DeleteHints();
            SpawnHints();
        }

        private void AttendTilePlayed(uint tileID)
        {
            DeleteHints();
            SpawnHints();
        }

        private void AttendSongEnd(string songName)
        {
            DeleteHints();
        }
    }
}