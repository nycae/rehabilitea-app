using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HideAndSeek 
{
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider                 slider      = null;
        [SerializeField] private HideAndSeekGameMode    gameMode    = null;
        [SerializeField] private RehabiliTEA.Player     player      = null;
        [SerializeField] private GameObject             optionsMenu = null;
                         private float                  timestamp   = 0f;   

        void ResetTimestamp(GameObject gameObject = null)
        {
            timestamp = Time.time;
            optionsMenu.SetActive(false);
            slider.gameObject.SetActive(true);
        }

        void HideBar()
        {
            slider.gameObject.SetActive(false);
        }

        void Start()
        {
            player.OnSelect     += ResetTimestamp;
            gameMode.OnGameEnd  += HideBar;

            optionsMenu.SetActive(false);
            ResetTimestamp();
        }

        void Update()
        {
            float progres = (Time.time - timestamp) / gameMode.GetSecondsToWait();

            if(progres < 1f)
            {
                slider.value = progres;
            }
            else
            {
                if( !optionsMenu.activeSelf )
                {
                    optionsMenu.SetActive(true);
                    slider.gameObject.SetActive(false);
                }
            }
        }
    }
}