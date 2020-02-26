using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.Framework
{

public class MemoryGameManager : MonoBehaviour
{
    public enum Difficulty
    {
        Easy, Medium, Hard, MAX
    }

    [System.Serializable]
    public struct DifficultyAssign
    {
        public Difficulty diff;
        public int cardSpawns;
    }

    public delegate void                    EvaluatePair(bool wasSuccessful);

    static public event EvaluatePair        OnPair;

    public delegate void                    EndGame();

    static public event EndGame             OnEndGame;


    private int                             remainingPairs      = 0;

    [SerializeField]
    private Difficulty                      gameDifficulty      = Difficulty.Medium;

    [SerializeField]
    private MemorySpawnManager              spawnManager        = null;

    [SerializeField]
    private DifficultyAssign[]              difficultySpawnList;

    [SerializeField]
    private Dictionary<Difficulty, int>     difficultySpawns    = new Dictionary<Difficulty, int>();

    private NPC.MemoryCard                  selectedCard        = null;

    private NPC.MemoryCard                  secondCard          = null;

    private void Awake() 
    {
        foreach(var item in difficultySpawnList)
        {
            difficultySpawns.Add(item.diff, item.cardSpawns);
        }

        remainingPairs = difficultySpawns[gameDifficulty] / 2;

        spawnManager.SpawnCards(difficultySpawns[gameDifficulty]);    
    }

    private void Start()
    {
        foreach (NPC.MemoryCard card in FindObjectsOfType<NPC.MemoryCard>())
        {
            card.OnSelect += OnCardTurned;
        }
    }

    private void OnCardTurned(NPC.MemoryCard card)
    {
        if (selectedCard == null)
        {
            selectedCard = card;
        }
        else
        {
            if (card.figure == selectedCard.figure)
            {
                OnPair.Invoke(true);
                DeselectCards();
                remainingPairs--;

                if (remainingPairs <= 0)
                {
                    OnEndGame.Invoke();
                }
            }
            else
            {
                OnPair.Invoke(false);
                secondCard = card;
                Invoke("FlipCards", NPC.MemoryCard.timeToTurn * 1.5f);
            }
        }
    }

    void FlipCards()
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
            remainingPairs--;
        }
    }
}   

}
