using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace RehabiliTEA
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField]
        protected GameObject    messageScreen;
        protected Player        player;
        protected AudioManager  audioManager;
        protected int           failedRounds;
        protected int           maxFailedRounds;
        protected Difficulty    difficulty;
        public delegate void    EndGame();
        public event EndGame    OnGameEnd;

        private void Awake()
        {
            player       = FindObjectOfType<Player>();
            audioManager = FindObjectOfType<AudioManager>();
            difficulty   = Profile.GetProfile().TaskDifficulty;
            OnGameEnd   += CleanupGame;

            Assert.IsNotNull(player);
            Assert.IsNotNull(audioManager);

            if (messageScreen) messageScreen.SetActive(false);
            
            StartCoroutine(Profile.GetProfile().PostEvent("GameStart"));
        }

        protected void FinishGame()
        {
            OnGameEnd?.Invoke();
        }

        private void CleanupGame()
        {
            StartCoroutine(Profile.GetProfile().PostScore(failedRounds, maxFailedRounds));
            
            if (failedRounds < maxFailedRounds)
            {
                PlayGoodEndAnimation();
                audioManager.PlayPositiveEndgameSound();
                Profile.GetProfile().UpdateDifficulty();
                Profile.GetProfile().PostEvent("GamePassed");
            }
            else
            {
                PlayBadEndAnimation();
                audioManager.PlayNegativeEndgameSound();
                Profile.GetProfile().PostEvent("GameFailed");
            }
            
            Invoke(nameof(LoadMainMenu), 5f);
        }

        private void PlayGoodEndAnimation()
        {
            if (!messageScreen) return;
            var message = "¡Muy bien, sigue así!\n" + 
                          $"Puntuación: {failedRounds} de {maxFailedRounds} fallos";
            
            messageScreen.GetComponentInChildren<Text>().text = message;
            messageScreen.SetActive(true);
        }

        private void PlayBadEndAnimation()
        {
            if (!messageScreen) return;
            var message = "¡Puedes mejorar, vuelve a intentarlo!\n"+
                          $"Puntuación:{failedRounds} de {maxFailedRounds} fallos";

            messageScreen.GetComponentInChildren<Text>().text = message;
            messageScreen.SetActive(true);
        }

        private void LoadMainMenu()       { SceneManager.LoadScene("MainMenu"); }
        public Difficulty GetDifficulty() { return difficulty; }
    }
}