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

        private void Start()
        {
            DisplayMainCanvas();
        }

        // public void DisplayMainCanvas()
        // {
        //     mainCanvas.enabled      = true;
        //     gameCanvas.enabled      = false;
        //     configCanvas.enabled    = false;
        // }

        // public void DisplayGameCanvas()
        // {
        //     mainCanvas.enabled      = false;
        //     gameCanvas.enabled      = true;
        //     configCanvas.enabled    = false;
        // }

        // public void DisplayConfigCanvas()
        // {
        //     mainCanvas.enabled      = false;
        //     gameCanvas.enabled      = false;
        //     configCanvas.enabled    = true;
        // }

        public void DisplayMainCanvas()
        {
            mainCanvas.gameObject.SetActive(true);
            gameCanvas.gameObject.SetActive(false);
            configCanvas.gameObject.SetActive(false);
        }

        public void DisplayGameCanvas()
        {
            mainCanvas.gameObject.SetActive(false);
            gameCanvas.gameObject.SetActive(true);
            configCanvas.gameObject.SetActive(false);
        }

        public void DisplayConfigCanvas()
        {
            mainCanvas.gameObject.SetActive(false);
            gameCanvas.gameObject.SetActive(false);
            configCanvas.gameObject.SetActive(true);
        }
    }
}
