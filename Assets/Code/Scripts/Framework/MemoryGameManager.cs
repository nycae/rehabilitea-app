using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.Framework;

namespace Memory.Framework
{

public class MemoryGameManager : MonoBehaviour
{

    [System.Serializable]
    public struct DifficultyAssign
    {
        public Difficulty   diff;
        public int          cardSpawns;
    }

    public delegate void                    EvaluatePair(bool wasSuccessful);

    static public event EvaluatePair        OnPair;

    public delegate void                    EndGame();

    static public event EndGame             OnEndGame;

    private int                             currentPairs        = 0;

    private int                             targetPairs         = 0;

    [SerializeField]
    private Difficulty                      gameDifficulty      = Difficulty.Medium;

    [SerializeField]
    private MemorySpawnManager              spawnManager        = null;

    [SerializeField]
    private DifficultyAssign[]              difficultySpawnList = new DifficultyAssign[0];

    [SerializeField]
    private Dictionary<Difficulty, int>     difficultySpawns    = new Dictionary<Difficulty, int>();

    private NPC.MemoryCard                  selectedCard        = null;

    private NPC.MemoryCard                  secondCard          = null;

    private void Awake() 
    {
        foreach (var item in difficultySpawnList)
        {
            difficultySpawns.Add(item.diff, item.cardSpawns);
        }

        targetPairs = difficultySpawns[gameDifficulty] / 2;

        spawnManager.SpawnCards(difficultySpawns[gameDifficulty]);    
    }

    private void Start()
    {
        foreach (NPC.MemoryCard card in FindObjectsOfType<NPC.MemoryCard>())
        {
            card.OnSelect += OnCardTurned;
        }

        OnPair      += AttendPair;
        OnEndGame   += LoadMainMenu;
    }

    private void OnCardTurned(NPC.MemoryCard card)
    {
        if (card == selectedCard) return;
        
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
        if (secondCard.GetFigure() == selectedCard.GetFigure())
        {
            OnPair.Invoke(true);
            DeselectCards();

            if (currentPairs == targetPairs)
            {
                OnEndGame.Invoke();
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
        return NPC.MemoryCard.timeToTurn;
    }

    private void LoadMainMenu()
    {
        Debug.Log("Loading main menu");
    }
}   

}
