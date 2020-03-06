using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.Framework
{

public class MemoryEventAnouncer : MonoBehaviour
{
    private void Awake() 
    {
        foreach (NPC.MemoryCard card in FindObjectsOfType<NPC.MemoryCard>())
        {
            card.OnSelect += OnCardSelected;
        }

        MemoryGameManager.OnPair += OnPairRecieved;
    }

    void OnCardSelected(NPC.MemoryCard card)
    {

    }

    void OnPairRecieved(bool wasSuccessful)
    {

    }
}

}