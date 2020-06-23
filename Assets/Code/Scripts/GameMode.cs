using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace RehabiliTEA
{
    public class GameMode : MonoBehaviour
    {
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
            }
            else
            {
                PlayBadEndAnimation();
                audioManager.PlayNegativeEndgameSound();
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
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}