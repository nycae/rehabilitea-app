using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HideAndSeek 
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider                 slider      = null;
        [SerializeField] private HideAndSeekGameMode    gameMode    = null;
        [SerializeField] private RehabiliTEA.Player     player      = null;
        [SerializeField] private CanvasGroup            flashScreen = null;
        [SerializeField] private GameObject             optionsMenu = null;
                         private float                  timestamp   = 0f;   

        void FlashMessage(bool wasSuccessfull)
        {
            flashScreen.GetComponentInChildren<Text>().text = (wasSuccessfull) ? "¡Bien hecho!" : "¡Prueba otra vez!" ;
            DisplayMessage();
            StartCoroutine("FadeOutMessage");
        }

        void DisplayMessage()
        {
            flashScreen.alpha = 1f;
        }

        IEnumerator FadeOutMessage()
        {
            for (float f = 1f; f > 0; f -= 0.005f)
            {
                flashScreen.alpha = f;
                yield return new WaitForSeconds(0.005f);
            }
        }

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
            gameMode.OnNewRound += FlashMessage;
            flashScreen.alpha   = 0f;

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