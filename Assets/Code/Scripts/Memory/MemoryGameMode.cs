using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace Memory
{
    public class MemoryGameMode : GameMode
    {
        [System.Serializable]
        private class DifficultyAssign
        {
            public Difficulty               diff                = Difficulty.Default;
            [Range(1, 200)] public int      maxFailures         = 0;
            [Range(0, 128)] public int      cardSpawns          = 0;
        }

        [SerializeField]
        private MemorySpawnManager          spawnManager        = null;

        [SerializeField]
        private DifficultyAssign[]          difficultySpawnList = null;

        private MemoryCard                  selectedCard        = null;
        private MemoryCard                  secondCard          = null;
        private int                         currentPairs        = 0;
        private int                         targetSpawns        = 0;
        private int                         targetPairs         = 0;
        public delegate void                EvaluatePair(bool wasSuccessful);
        static public event EvaluatePair    OnPair;


        private void Start()
        {
            foreach (var item in difficultySpawnList)
            {
                if (item.diff == difficulty)
                {
                    targetSpawns    = item.cardSpawns;
                    maxFailedRounds = item.maxFailures;
                    break;
                }
            }

            spawnManager.SpawnCards(targetSpawns);

            foreach (MemoryCard card in FindObjectsOfType<MemoryCard>())
            {
                card.OnSelect += OnCardTurned;
            }

            OnPair             += AttendPair;
            difficultySpawnList = null;
            targetPairs         = targetSpawns / 2;
        }

        private void OnCardTurned(MemoryCard card)
        {
            if (card == selectedCard) return;

            audioManager.PlayEnvironmentSound("CardFlip");

            if (selectedCard == null)
            {
                selectedCard = card;
            }
            else
            {
                secondCard = card;
                CheckCards();
            }
        }

        private void CheckCards()
        {
            if (secondCard.GetSpriteRenderer().sprite == selectedCard.GetSpriteRenderer().sprite)
            {
                OnPair.Invoke(true);
                audioManager.PlayRandomRewardingSound();
                DeselectCards();

                if (currentPairs == targetPairs)
                {
                    FinishGame();
                }
            }
            else
            {
                OnPair.Invoke(false);
                Invoke("FlipCards", GetWaitTime());
            }
        }

        private void FlipCards()
        {
            selectedCard.TurnArround();
            secondCard.TurnArround();

            DeselectCards();
        }

        void DeselectCards()
        {
            selectedCard = null;
            selectedCard = null;
        }

        void AttendPair(bool wasSuccessful)
        {
            if (wasSuccessful)
            {
                currentPairs++;
            }
            else
            {
                failedRounds++;
            }
        }

        public int GetTargetScore()
        {
            return targetPairs;
        }

        public int GetCurrentScore()
        {
            return currentPairs;
        }

        public float GetWaitTime()
        {
            return MemoryCard.TimeToTurn * 1.5f;
        }
    }
}
