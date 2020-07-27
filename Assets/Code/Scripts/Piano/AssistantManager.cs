using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Piano
{
    public class AssistantManager : MonoBehaviour
    {
        [System.Serializable] private class NoteLocation
        {
            public uint     id          = 0;
            public Vector3  location    = new Vector3();
        }

        [SerializeField] private Sprite[]       spriteNumber        = null;
        [SerializeField] private NoteLocation[] noteLocationBuilder = null;
        [SerializeField] private SongManager    songManager         = null;
        [SerializeField] private int            maxHints            = 0;

        private GameObject[]                    hints               = null;
        private Dictionary<uint, Vector3>       noteLocations       = new Dictionary<uint, Vector3>();
        private Vector3                         numberScale         = new Vector3(0.25f, 0.25f, 1f);

        void Start()
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

        void SpawnHints()
        {
            int     hintCount       = System.Math.Min(maxHints, songManager.GetRemainingNotes());
            uint[]  songSequence    = songManager.GetSequence().Distinct().Take(hintCount).ToArray();
                    hints           = new GameObject[maxHints];

            for (int i = 0; i < System.Math.Min(maxHints, songSequence.Count()); i++)
            {
                hints[i]                        = new GameObject();
                hints[i].transform.position     = noteLocations[songSequence[i]];
                hints[i].transform.localScale   = numberScale;
                
                var spriteRendererBuffer        = hints[i].AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                spriteRendererBuffer.sprite     = spriteNumber[i];
            }
        }

        void DeleteHints()
        {
            if (hints != null)
            {
                foreach (var gameObject in hints)
                {
                    Destroy(gameObject);
                }
            }
        }

        void AttendSongBegin(string songName)
        {
            DeleteHints();
            SpawnHints();
        }

        void AttendTilePlayed(uint tileID)
        {
            DeleteHints();
            SpawnHints();
        }

        void AttendSongEnd(string songName)
        {
            DeleteHints();
        }
    }
}