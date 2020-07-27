using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Piano
{
    public class UICanvas : MonoBehaviour
    {
        [SerializeField] private Canvas         songCanvas      = null;
        [SerializeField] private Canvas         buttonCanvas    = null;
        [SerializeField] private SongManager    songManager     = null;

        void Start()
        {
            HideSongCanvas();
        }

        public void DisplaySongCanvas()
        {
            songCanvas.gameObject.SetActive(true);
            buttonCanvas.gameObject.SetActive(false);
        }

        public void HideSongCanvas()
        {
            songCanvas.gameObject.SetActive(false);
            buttonCanvas.gameObject.SetActive(true);
        }

        public void LoadSong(string songName)
        {
            songManager.PlaySong(songName);
        }
    }
}