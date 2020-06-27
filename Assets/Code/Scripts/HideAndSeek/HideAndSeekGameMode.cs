using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace HideAndSeek
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

        [SerializeField]
        private UnityEngine.UI.Slider   progressBar     = null;

        private Sprite[]                levelSprites    = null;
        private Sprite[]                selectables     = null;
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
            difficulties                = null; // Free memory, may cause a memory leak, do some research later.

            player.OnSelect            += Evaluate;
            OnNewRound                 += PrepareRound;

            StartRound();
        }

        private void StartRound()
        {
            spawnManager.DestroyAll();

            Player.SecondsWhenBlocked = secondsToWait;
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
            switch (difficulty)
            {
                case Difficulty.Easy: 
                    selectables     = assetManager.GetRandomShapes(3);
                    levelSprites    = assetManager.GetRandomShapes(figuresToSpawn);
                    break;

                case Difficulty.Medium:
                    selectables     = assetManager.GetRandomShapesAndCharacters(3);
                    levelSprites    = assetManager.GetRandomShapesAndCharacters(figuresToSpawn);
                    break;

                case Difficulty.Hard:
                    selectables     = assetManager.GetRandomSprites(3);
                    levelSprites    = assetManager.GetRandomSprites(figuresToSpawn);
                    break;
            }

            foreach (var sprite in levelSprites)
            {
                spawnManager.Spawn(sprite);
            }

            targetSprite = levelSprites[Random.Range(0, levelSprites.Length - 1)];
        }

        private void Evaluate(GameObject sprite)
        {
            audioManager.PlayEnvironmentSound("Selection");
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
            int     j               = 0;
            bool    isTargetVisible = false;
            Vector3 startingPoint   = new Vector3(-4.3f, -3.7f, 0.0f);

            for (int i = 0; i < 4; i++)
            {
                Vector3 spawnPoint  = startingPoint;
                spawnPoint.x        -= -2.7666666666666666666666666666667f * i;

                if (i >= 3 && !isTargetVisible)
                {
                    spawnManager.Spawn(targetSprite, spawnPoint);
                }
                else
                {
                    if (isTargetVisible || Random.Range(0f, 1f) <= 0.5f)
                    {
                        spawnManager.Spawn(selectables[j], spawnPoint);
                        j++;
                    }
                    else
                    {
                        spawnManager.Spawn(targetSprite, spawnPoint);
                        isTargetVisible = true;
                    }
                }
            }
        }

        public float GetSecondsToWait()
        {
            return secondsToWait;
        }
    }
}
