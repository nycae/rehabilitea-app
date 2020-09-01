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
            player          = FindObjectOfType<Player>();
            audioManager    = FindObjectOfType<AudioManager>();
            difficulty      = RehabiliTEA.Profile.GetProfile().GetDifficulty();
            OnGameEnd      += CleanupGame;

            Assert.IsNotNull(player);
            Assert.IsNotNull(audioManager);

            if (messageScreen) messageScreen.SetActive(false);
            Profile.GetProfile().PostEvent("GameStart");
        }

        protected void FinishGame()
        {
            OnGameEnd.Invoke();
        }

        private void CleanupGame()
        {
            if (failedRounds < maxFailedRounds)
            {
                if (difficulty < Difficulty.Hard) difficulty++;

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

            Invoke("LoadMainMenu", 5f);
        }

        private void PlayGoodEndAnimation()
        {
            if (messageScreen)
            {
                messageScreen.GetComponentInChildren<Text>().text = 
                    System.String.Format("¡Muy bien, sigue así!\nPuntuación: {0} de {1} fallos", failedRounds, maxFailedRounds);
                messageScreen.SetActive(true);
            }
        }

        private void PlayBadEndAnimation()
        {
            if (messageScreen)
            {
                messageScreen.GetComponentInChildren<Text>().text = 
                    System.String.Format("¡Puedes mejorar, vuelve a intentarlo!\nPuntuación: {0} de {1} fallos", failedRounds, maxFailedRounds);
                messageScreen.SetActive(true);
            }
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu"); 
        }

        public Difficulty GetDifficulty()
        {
            return difficulty;
        }
    }
}