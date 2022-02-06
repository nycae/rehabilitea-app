using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HideAndSeek 
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider              slider;
        [SerializeField] private HideAndSeekGameMode gameMode;
        [SerializeField] private RehabiliTEA.Player  player;
        [SerializeField] private CanvasGroup         flashScreen;
        [SerializeField] private GameObject          optionsMenu;
        private                  float               timestamp;

        private void FlashMessage(bool wasSuccessful)
        {
            flashScreen.GetComponentInChildren<Text>().text = wasSuccessful ? "¡Bien hecho!" : "¡Prueba otra vez!" ;
            DisplayMessage();
            StartCoroutine(nameof(FadeOutMessage));
        }

        private void DisplayMessage()
        {
            flashScreen.alpha = 1f;
        }

        private IEnumerator FadeOutMessage()
        {
            yield return new WaitForSeconds(1f);
            
            for (var f = 1f; f > 0; f -= 0.005f)
            {
                flashScreen.alpha = f;
                yield return new WaitForSeconds(0.005f);
            }
        }

        private void ResetTimestamp(GameObject gameObj = null)
        {
            timestamp = Time.time;
            optionsMenu.SetActive(false);
            slider.gameObject.SetActive(true);
        }

        private void HideBar()
        {
            slider.gameObject.SetActive(false);
        }

        private void Start()
        {
            player.OnSelect     += ResetTimestamp;
            gameMode.OnGameEnd  += HideBar;
            gameMode.OnNewRound += FlashMessage;
            flashScreen.alpha   = 0f;

            optionsMenu.SetActive(false);
            ResetTimestamp();
        }

        private void Update()
        {
            var progress = (Time.time - timestamp) / gameMode.GetSecondsToWait();

            if(progress < 1f)
            {
                slider.value = progress;
            }
            else
            {
                if (optionsMenu.activeSelf) return;
                optionsMenu.SetActive(true);
                slider.gameObject.SetActive(false);
            }
        }
    }
}