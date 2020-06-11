using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.Framework;
using RehabiliTEA.Player;

namespace HideAndSeek.Framework
{
    public class HideAndSeekGameMode : MonoBehaviour
    {
        [System.Serializable]
        private class DifficultyAssign
        {
            public Difficulty           difficulty;
            [Range(1, 10)] public int   rounds;
            [Range(1, 20)] public int   figuresToSpawn;
            [Range(0, 10)] public int   figureAugment;
            [Range(1, 20)] public int   maxRoundsToFail;
            [Range(1, 20)] public float secondsToWait;
            [Range(0, 10)] public float timeDecrease;
        }

        [SerializeField]
        private DifficultyAssign[]      difficulties    = null;

        [SerializeField]
        private UserPlayer              player          = null;

        [SerializeField]
        private AssetManager            assetManager    = null;

        [SerializeField]
        private HideAndSeekSpawnManager spawnManager    = null;

        [SerializeField]
        private Difficulty              difficulty      = Difficulty.Default;
        private Sprite[]                levelSprites    = null;
        [SerializeField]
        private Sprite                  targetSprite    = null;
        private float                   secondsToWait   = 0f;
        private int                     rounds          = 0;
        private int                     failedRounds    = 0;
        private int                     maxFailedRounds = 0;
        private int                     figuresToSpawn  = 0;
        private int                     figuresAugment  = 0;
        private float                   timeDecrease    = 0;

        public delegate void            NewRound(bool wasSuccessfull);
        public delegate void            EndGame(bool wasSuccessfull);
        
        static public event NewRound    OnNewRound;
        static public event EndGame     OnGameEnd;

        private void Awake()
        {
            UserPlayer.OnSelect += Evaluate;
            OnNewRound          += PrepareRound;
            OnGameEnd           += FinishGame;

            foreach (var assignament in difficulties)
            {
                if (assignament.difficulty == this.difficulty)
                {
                    rounds          = assignament.rounds;
                    timeDecrease    = assignament.timeDecrease;
                    secondsToWait   = assignament.secondsToWait;
                    figuresAugment  = assignament.figureAugment;
                    figuresToSpawn  = assignament.figuresToSpawn;
                    maxFailedRounds = assignament.maxRoundsToFail;

                    break;
                }
            }
            difficulties = null; // Free memory, may cause a memory leak, do some research later.

            StartRound();
        }

        private void StartRound()
        {
            UserPlayer.SecondsWhenBlocked = secondsToWait;

            spawnManager.DestroyAll();
            player.Block();
            GenerateSequence();
            Invoke("HideTarget", secondsToWait);
        }

        private void PrepareRound(bool wasSuccessfull)
        {
            Debug.Log(wasSuccessfull);
            if (wasSuccessfull) // User was right.
            {
                rounds--;

                if (rounds > 0) // If there is more rounds increase difficulty and start again.
                {
                    secondsToWait -= timeDecrease;
                    figuresToSpawn += figuresAugment;
                    StartRound();
                }
                else // Otherwise end the game.
                {
                    spawnManager.DestroyAll();
                    OnGameEnd.Invoke(failedRounds < maxFailedRounds);
                }
            }
            else // User failed, try again with the same values.
            {
                failedRounds++;
                StartRound();
            }
        }

        private void GenerateSequence()
        {
            levelSprites = assetManager.GetRandomShapes(figuresToSpawn);

            foreach (var sprite in levelSprites)
            {
                spawnManager.Spawn(sprite);
            }

            targetSprite = levelSprites[Random.Range(0, levelSprites.Length - 1)];
        }

        private void Evaluate(Sprite sprite)
        {
            OnNewRound.Invoke(sprite == targetSprite);
        }

        private void HideTarget()
        {
            foreach (var spriteRenderer in FindObjectsOfType<SpriteRenderer>())
            {
                if (spriteRenderer.sprite == targetSprite)
                {
                    spriteRenderer.enabled = false;
                    break;
                }
            }

            SpawnOptions();
        }

        private void SpawnOptions()
        {
            Vector3 startingPoint   = new Vector3(7.85f, 2.25f, 0.0f);
            bool    isTargetVisible = false;

            for (int i = 0; i < 4; i++)
            {
                Vector3 spawnPoint  = startingPoint;
                spawnPoint.y        -= 2f * i;

                if (i >= 3 && !isTargetVisible)
                {
                    spawnManager.Spawn(targetSprite, spawnPoint);
                }
                else
                {
                    if (isTargetVisible || Random.Range(0f, 1f) <= 0.5f)
                    {
                        spawnManager.Spawn(assetManager.GetRandomShapes(50)[Random.Range(0, 49)], spawnPoint);
                    }
                    else
                    {
                        spawnManager.Spawn(targetSprite, spawnPoint);
                        isTargetVisible = true;
                    }
                }
            }
        }

        private void FinishGame(bool wasSuccessfull)
        {
            if (wasSuccessfull && RehabiliTEA.Profile.GetProfile().GetDifficulty() < Difficulty.Hard)
            {
                Difficulty newDifficulty = RehabiliTEA.Profile.GetProfile().GetDifficulty() + 1;
                // Set new difficulty
            }
            // Load Main menu
        }
    }
}
