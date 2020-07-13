using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RehabiliTEA
{
    public class UIButtonManager : MonoBehaviour
    {
        [SerializeField] private Canvas mainCanvas      = null;
        [SerializeField] private Canvas gameCanvas      = null;
        [SerializeField] private Canvas configCanvas    = null;
        [SerializeField] private Canvas profileCanvas   = null;

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
            if (!Profile.GetProfile().HasInternetConnection())
            {
                HideAll();
                configCanvas.gameObject.SetActive(true);
            }
        }

        public void DisplayProfileCanvas()
        {
            HideAll();
            profileCanvas.gameObject.SetActive(true);
        }

        public void SaveProfileData(UnityEngine.UI.InputField inputField)
        {
            int newId = Profile.GetProfile().GetId();

            try 
            {   
                newId = int.Parse(inputField.text);
            }
            catch (System.FormatException)
            {
                // pass
            }

            Profile.GetProfile().SetId(newId);
        }
    }
}
