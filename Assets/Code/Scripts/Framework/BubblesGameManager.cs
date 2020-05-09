using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.Framework;

namespace Bubbles.Framework
{


public class BubblesGameManager : MonoBehaviour
{
    [System.Serializable]
    public struct DifficultyAssign
    {
        public Difficulty       difficulty;
        
        [Range(0, 10)]
        public int              numberOfBubbles;

        [Range(0, 10)]
        public int              numberOfDisctractions;
    }

    [SerializeField]
    private DifficultyAssign[]  difficulties    = null;

    [SerializeField]
    private Difficulty          difficulty      = Difficulty.Default;

    [SerializeField]
    private BubblesSpawnManager spawnManager    = null;

    [SerializeField, Range(0.1f, 10.0f)]
    private float               spawnDelay      = 0.5f;

    private Queue<Sprite>       sequence        = null;

    private DifficultyAssign    settings        = new DifficultyAssign();

    private int                 spawnedSprites  = 0;

    private int                 hits            = 0;

    private int                 misses          = 0;

    public delegate void        Score();

    public delegate void        Fail();

    public delegate void        Endgame();

    public static event Endgame OnEndGame;

    public static event Score   OnScore;

    public static event Fail    OnFail;


    void Awake()
    {
        foreach (var difficultyAssign in difficulties)
        {
            if (difficultyAssign.difficulty == this.difficulty)
            {
                this.settings = difficultyAssign;
                break;
            }
        }

        OnEndGame += LoadMainMenu;
    }

    void Start()
    {
        sequence = new Queue<Sprite>(spawnManager.GenerateSpriteSequences(settings.numberOfBubbles, settings.numberOfDisctractions));

        Invoke("SpawnNextSprite", spawnDelay);
    }

    void AttendBubble(NPC.Bubble bubble)
    {
        if (sequence.Peek() == bubble.GetSprite())
        {
            Destroy(bubble.gameObject);

            sequence.Dequeue();

            hits++;

            if (hits >= settings.numberOfBubbles)
            {
                OnEndGame.Invoke();
            }
        }
        else
        {
            misses++;
        }
    }

    void SpawnNextSprite()
    {
        spawnManager.SpawnNextSprite().GetComponent<NPC.Bubble>().OnSelect += AttendBubble;
        spawnedSprites++;

        if (spawnedSprites < settings.numberOfBubbles + settings.numberOfDisctractions)
        {
            Invoke("SpawnNextSprite", spawnDelay);
        }
    }

    void LoadMainMenu()
    {
        Debug.Log("Loading main menu");
    }
}


}
