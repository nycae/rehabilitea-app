using UnityEngine;
using UnityEngine.UI;

namespace RehabiliTEA
{
    public class UIButtonManager : MonoBehaviour
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas gameCanvas;
        [SerializeField] private Canvas configCanvas;
        [SerializeField] private Canvas profileCanvas;

        private void Start()
        {
            DisplayMainCanvas();
        }

        private void HideAll()
        {
            mainCanvas.gameObject.SetActive(false);
            gameCanvas.gameObject.SetActive(false);
            configCanvas.gameObject.SetActive(false);
            profileCanvas.gameObject.SetActive(false);
        }

        public void DisplayMainCanvas()
        {
            HideAll();
            mainCanvas.gameObject.SetActive(true);
        }

        public void DisplayGameCanvas()
        {
            HideAll();
            gameCanvas.gameObject.SetActive(true);
        }

        public void DisplayConfigCanvas()
        {
            if (Profile.GetProfile().HasInternet) return;
            
            HideAll();
            configCanvas.gameObject.SetActive(true);
        }

        public void DisplayProfileCanvas()
        {
            HideAll();
            profileCanvas.gameObject.SetActive(true);
        }

        public void SaveProfileData(InputField idField)
        {
            try
            {
                Profile.GetProfile().ID = int.Parse(idField.text);
            }
            catch (System.FormatException)
            {
               // pass 
            }
        }

        public void SaveUrlData(InputField urlField) { GreydoAPI.BaseURL = urlField.text; }

        public void TestInternetConnection(Text text)
        {
            Profile.GetProfile().CheckInternet();
            text.text = Profile.GetProfile().HasInternet ? "tengo internet" : "no tengo internet";
        }
    }
}
