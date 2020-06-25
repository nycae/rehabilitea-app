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
                         private float                  timestamp   = 0f;   

        void ResetTimestamp(GameObject gameObject)
        {
            timestamp = Time.time;
        }

        void HideBar()
        {
            slider.gameObject.SetActive(false);
        }

        void Start()
        {
            player.OnSelect     += ResetTimestamp;
            gameMode.OnGameEnd  += HideBar;
        }

        void Update()
        {
            slider.value = (Time.time - timestamp) / gameMode.GetSecondsToWait();
        }
    }
}