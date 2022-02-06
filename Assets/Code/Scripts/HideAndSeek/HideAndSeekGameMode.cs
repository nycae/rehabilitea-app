using System;
using UnityEngine;
using RehabiliTEA;
using Random = UnityEngine.Random;

namespace HideAndSeek
{
    public class HideAndSeekGameMode : GameMode
    {
        [Serializable]
        private class DifficultyAssign
        {
            public Difficulty difficulty = Difficulty.Hard;
            
            [Range(1, 10)] public int        rounds;
            [Range(1, 20)] public int        figuresToSpawn;
            [Range(0, 10)] public int        figureAugment;
            [Range(1, 20)] public int        maxRoundsToFail;
            [Range(1, 20)] public float      secondsToWait;
            [Range(0, 10)] public float      timeDecrease;
        }

        [SerializeField] private DifficultyAssign[]      difficulties;
        [SerializeField] private AssetManager            assetManager;
        [SerializeField] private HideAndSeekSpawnManager spawnManager;

        private Sprite[] levelSprites;
        private Sprite[] selectables;
        private Sprite   targetSprite;
        private float    secondsToWait;
        private int      rounds;
        private int      figuresToSpawn;
        private int      figuresAugment;
        private float    timeDecrease;

        public delegate void  NewRound(bool wasSuccessful);
        public event NewRound OnNewRound;

        private void Start()
        {
            foreach (var assignment in difficulties)
            {
                if (assignment.difficulty != difficulty) continue;

                rounds          = assignment.rounds;
                timeDecrease    = assignment.timeDecrease;
                secondsToWait   = assignment.secondsToWait;
                figuresAugment  = assignment.figureAugment;
                figuresToSpawn  = assignment.figuresToSpawn;
                maxFailedRounds = assignment.maxRoundsToFail;

                break;
            }

            difficulties    = null; // Free memory, may cause a memory leak, do some research later.
            player.OnSelect += Evaluate;
            OnNewRound      += PrepareRound;

            StartRound();
        }

        private void StartRound()
        {
            spawnManager.DestroyAll();

            //Player.SecondsWhenBlocked = secondsToWait;
            player.Block(secondsToWait);

            GenerateSequence();
            Invoke(nameof(HideTarget), secondsToWait);
        }

        private void PrepareRound(bool wasSuccessful)
        {
            if (wasSuccessful) // User was right.
            {
                rounds--;

                if (rounds > 0) // If there is more rounds increase difficulty and start again.
                {
                    secondsToWait   -= timeDecrease;
                    figuresToSpawn  += figuresAugment;
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

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var sprite in levelSprites)  spawnManager.Spawn(sprite);

            targetSprite = levelSprites[Random.Range(0, levelSprites.Length - 1)];
        }

        private void Evaluate(GameObject sprite)
        {
            audioManager.PlayEnvironmentSound("Selection");
            OnNewRound?.Invoke(sprite.GetComponent<SpriteRenderer>().sprite == targetSprite);
        }

        private void HideTarget()
        {
            foreach (var spriteRenderer in FindObjectsOfType<SpriteRenderer>())
            {
                if (spriteRenderer.sprite != targetSprite) continue;
                
                spriteRenderer.enabled = false;
                break;
            }

            SpawnOptions();
        }

        private void SpawnOptions()
        {
            var j               = 0;
            var isTargetVisible = false;
            var startingPoint   = new Vector3(-4.3f, -3.7f, 0.0f);

            for (var i = 0; i < 4; i++)
            {
                var spawnPoint  = startingPoint;
                spawnPoint.x        -= -2.7666666666666666666666666666667f * i;

                if (i >= 3 && !isTargetVisible)
                {
                    spawnManager.SpawnClickable(targetSprite, spawnPoint);
                }
                else
                {
                    if (isTargetVisible || Random.Range(0f, 1f) <= 0.5f)
                    {
                        spawnManager.SpawnClickable(selectables[j], spawnPoint);
                        j++;
                    }
                    else
                    {
                        spawnManager.SpawnClickable(targetSprite, spawnPoint);
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
