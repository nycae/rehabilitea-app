using UnityEngine;

namespace Piano
{
    public class UICanvas : MonoBehaviour
    {
        [SerializeField] private Canvas      songCanvas;
        [SerializeField] private Canvas      buttonCanvas;
        [SerializeField] private SongManager songManager;

        private void Start()
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