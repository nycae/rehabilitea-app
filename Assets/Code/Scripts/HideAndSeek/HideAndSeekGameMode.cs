using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RehabiliTEA.HideAndSeek
{
    public class HideAndSeekGameMode : GameMode
    {
        [System.Serializable]
        private class DifficultyAssign
        {
            public Difficulty           difficulty      = Difficulty.Default;
            [Range(1, 10)] public int   rounds          = 0;
            [Range(1, 20)] public int   figuresToSpawn  = 0;
            [Range(0, 10)] public int   figureAugment   = 0;
            [Range(1, 20)] public int   maxRoundsToFail = 0;
            [Range(1, 20)] public float secondsToWait   = 0f;
            [Range(0, 10)] public float timeDecrease    = 0f;
        }

        [SerializeField]
        private DifficultyAssign[]      difficulties    = null;

        [SerializeField]
        private AssetManager            assetManager    = null;

        [SerializeField]
        private HideAndSeekSpawnManager spawnManager    = null;

        private Sprite[]                levelSprites    = null;
        private Sprite                  targetSprite    = null;
        private float                   secondsToWait   = 0f;
        private int                     rounds          = 0;
        private int                     figuresToSpawn  = 0;
        private int                     figuresAugment  = 0;
        private float                   timeDecrease    = 0;
        public delegate void            NewRound(bool wasSuccessfull);
        public event NewRound           OnNewRound;

        private void Start()
        {
            Player.SecondsWhenBlocked   = secondsToWait;
            player.OnSelect            += Evaluate;
            OnNewRound                 += PrepareRound;

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
            spawnManager.DestroyAll();
            player.Block();
            GenerateSequence();
            Invoke("HideTarget", secondsToWait);
        }

        private void PrepareRound(bool wasSuccessfull)
        {
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
                    // Player.OnSelect -= Evaluate;
                    // OnNewRound      -= PrepareRound;
                    FinishGame();
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

        private void Evaluate(GameObject sprite)
        {
            OnNewRound.Invoke(sprite.GetComponent<SpriteRenderer>().sprite == targetSprite);
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
    }
}
