using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RehabiliTEA
{
    public class GameMode : MonoBehaviour
    {
        protected int           failedRounds;
        protected int           maxFailedRounds;
        protected Difficulty    difficulty;
        public delegate void    EndGame();
        public event EndGame    OnGameEnd;

        private void Awake()
        {
            difficulty  = RehabiliTEA.Profile.GetProfile().GetDifficulty();
            OnGameEnd  += CleanupGame;
        }

        protected void FinishGame()
        {
            this.OnGameEnd.Invoke();
        }

        private void CleanupGame()
        {
            if (failedRounds < maxFailedRounds)
            {
                if (difficulty < Difficulty.Hard) difficulty++;

                PlayGoodEndAnimation();
            }
            else
            {
                PlayBadEndAnimation();
            }

            Invoke("LoadMainMenu", 5f);
        }

        private void PlayGoodEndAnimation()
        {
            // TODO
        }

        private void PlayBadEndAnimation()
        {
            // TODO
        }

        private void LoadMainMenu()
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Loading main menu");
            } 
            else
            {
                SceneManager.LoadScene("MainMenu");
            }   
        }
    }
}