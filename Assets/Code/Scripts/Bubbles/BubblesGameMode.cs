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
            public Difficulty           difficulty          = 0;
            [Range(0, 10)] public int   bubbleCount         = 0;
            [Range(0, 10)] public int   distractionCount    = 0;
            [Range(0, 10)] public int   maxFailures         = 0;
        }

        [SerializeField]
        private DifficultyAssign[]      difficulties        = null;

        [SerializeField]
        private BubblesSpawnManager     spawnManager        = null;

        [SerializeField, Range(0.1f, 10.0f)]
        private float                   spawnDelay          = 0.5f;

        private Queue<Sprite>           sequence            = null;
        private int                     hits                = 0;
        private int                     spawnedSprites      = 0;
        private int                     bubbleCount         = 0;
        private int                     distractionCount    = 0;

        public delegate void            Score();
        public delegate void            Fail();

        public static event Score       OnScore;
        public static event Fail        OnFail;


        private void Start()
        {
            foreach (var difficultyAssign in difficulties)
            {
                if (difficultyAssign.difficulty == this.difficulty)
                {
                    this.bubbleCount        = difficultyAssign.bubbleCount;
                    this.distractionCount   = difficultyAssign.distractionCount;
                    this.maxFailedRounds    = difficultyAssign.maxFailures;

                    break;
                }
            }

            difficulties    = null;
            sequence        = new Queue<Sprite>(spawnManager.GenerateSpriteSequences(bubbleCount, distractionCount));

            player.OnSelect += AttendBubble;
            OnScore         += AddScore;
            OnFail          += AddMiss;

            Invoke("SpawnNextSprite", spawnDelay);
        }

        void AttendBubble(GameObject bubble)
        {
            if (sequence.Peek() == bubble.GetComponent<SpriteRenderer>().sprite)
            {
                Destroy(bubble);

                sequence.Dequeue();
                OnScore.Invoke();

                if (hits >= bubbleCount)
                {
                    FinishGame();
                }
            }
            else
            {
                OnFail.Invoke();
            }
        }

        void SpawnNextSprite()
        {
            spawnManager.SpawnNextSprite();
            spawnedSprites++;

            if (spawnedSprites < bubbleCount + distractionCount)
            {
                Invoke("SpawnNextSprite", spawnDelay);
            }
        }

        void AddScore()
        {
            hits++;
        }

        void AddMiss()
        {
            failedRounds++;
        }
    }
}
