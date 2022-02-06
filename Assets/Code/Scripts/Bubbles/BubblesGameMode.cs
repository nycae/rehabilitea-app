using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace RehabiliTEA.Bubbles
{
    public class BubblesGameMode : GameMode
    {
        [System.Serializable]
        private class DifficultyAssign
        {
            public Difficulty difficulty = 0;
            [Range(0, 20)]
            public int distractionCount;
            [Range(0, 20)]
            public int maxFailures;
        }

        [SerializeField]
        private DifficultyAssign[]      difficulties;

        [SerializeField]
        private BubblesSpawnManager     spawnManager;

        [SerializeField, Range(0.1f, 10.0f)]
        private float spawnDelay = 0.5f;

        private Queue<Sprite> sequence;
        private int           hits;
        private int           spawnedSprites;
        private int           bubbleCount;
        private int           distractionCount;

        public delegate void Score();
        public delegate void Fail();

        public static event Score OnScore;
        public static event Fail  OnFail;


        private void Start()
        {
            foreach (var difficultyAssign in difficulties)
            {
                if (difficultyAssign.difficulty != this.difficulty) continue;
                
                distractionCount = difficultyAssign.distractionCount;
                maxFailedRounds  = difficultyAssign.maxFailures;

                break;
            }

            difficulties = null;
            sequence = new Queue<Sprite>(
                    spawnManager.GenerateSpriteSequence(difficulty, distractionCount));
            bubbleCount = sequence.Count;

            player.OnSelect += AttendBubble;
            OnScore         += AddScore;
            OnFail          += AddMiss;

            Invoke(nameof(SpawnNextSprite), spawnDelay);
        }

        private void AttendBubble(GameObject bubble)
        {
            if (sequence.Peek() == bubble.GetComponent<SpriteRenderer>().sprite)
            {
                audioManager.PlayEnvironmentSound("BubbleExplosion");
                Destroy(bubble);
                sequence.Dequeue();
                OnScore?.Invoke();

                if (hits >= bubbleCount) FinishGame();
            }
            else
            {
                OnFail?.Invoke();
            }
        }

        private void SpawnNextSprite()
        {
            spawnManager.SpawnNextSprite();
            spawnedSprites++;

            if (spawnedSprites < bubbleCount + distractionCount)
            {
                Invoke(nameof(SpawnNextSprite), spawnDelay);
            }
        }

        private void AddScore()
        {
            hits++;
        }

        private void AddMiss()
        {
            failedRounds++;
        }
    }
}
