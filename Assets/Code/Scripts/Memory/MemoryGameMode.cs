using UnityEngine;
using RehabiliTEA;

namespace Memory
{
    public class MemoryGameMode : GameMode
    {
        [System.Serializable]
        private class DifficultyAssign
        {
            public Difficulty               diff                = Difficulty.Hard;
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
        public event EvaluatePair           OnPair;


        private void Start()
        {
            foreach (var item in difficultySpawnList)
            {
                if (item.diff != difficulty) continue;
                
                targetSpawns    = item.cardSpawns;
                maxFailedRounds = item.maxFailures;
                break;
            }

            spawnManager.SpawnCards(targetSpawns);

            foreach (var card in FindObjectsOfType<MemoryCard>())
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
                OnPair?.Invoke(true);
                audioManager.PlayRandomRewardingSound();
                DeselectCards();

                if (currentPairs == targetPairs)
                {
                    FinishGame();
                }
            }
            else
            {
                OnPair?.Invoke(false);
                Invoke(nameof(FlipCards), GetWaitTime());
            }
        }

        private void FlipCards()
        {
            selectedCard.TurnAround();
            secondCard.TurnAround();

            DeselectCards();
        }

        private void DeselectCards()
        {
            selectedCard = null;
            selectedCard = null;
        }

        private void AttendPair(bool wasSuccessful)
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

        private void OnDestroy()
        {
            selectedCard    = null;
            secondCard      = null;
            currentPairs    = 0;
            targetSpawns    = 0;
            targetPairs     = 0;
        }

        private static float GetWaitTime()
        {
            return MemoryCard.TimeToTurn * 1.5f;
        }
    }
}
