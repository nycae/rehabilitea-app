using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.Framework
{

public class MemorySpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[]    cardClasses; 

    [SerializeField]
    private GameObject      cardParent;

    [SerializeField]
    private Vector3         topRightCorner;

    [SerializeField]
    private float           widthOffset;

    [SerializeField]
    private float           heightOffset;

    [SerializeField]
    private int             cardsPerColumn;

    [SerializeField]
    private int             cardsPerRow;
    
    
    public void SpawnCards(int cardCount)
    {
        // We calculate the number of rows the cards will be distributed in
        int                     numberOfRows    = cardCount / cardsPerColumn;   
        // Creating a dictionary to store how many cards of each type have we spawned
        Dictionary<int, int>    classCount      = new Dictionary<int, int>();
        // Top right corner is prepared to be so for max number of cards, therefore it should be displace otherwise
        topRightCorner.x                       += (cardsPerRow - numberOfRows) / 2 * widthOffset;  
        // First spawn position is top right corner
        Vector3 spawnPosition                   = topRightCorner;

        // We spawn 2 cards of each type, therefore we only need to add half of the cards to the dictionary
        for (int i = 0; i < cardCount / 2; i++)
        {
            classCount.Add(i, 0);
        }

        for (int i = 0; i < cardsPerColumn; i++)
        {
            spawnPosition.x = topRightCorner.x;

            for (int j = 0; j < numberOfRows; j++)
            {
                // For each column, and for each row we generate a random index indicating a card type
                int classNumber = Random.Range(0, classCount.Count);

                // And change it if we spawned more than 1 card of that type
                while (classCount[classNumber] >= 2)
                {
                    classNumber = Random.Range(0, classCount.Count);
                }

                // We create the card based on the class, assign it to the father (so we can select all cards easily) and apply the transform
                var newCard = Instantiate(cardClasses[classNumber], spawnPosition, Quaternion.identity);

                newCard.transform.parent = cardParent.transform;

                classCount[classNumber]++;
                spawnPosition.x += widthOffset;
            }

            spawnPosition.y += heightOffset;
        }
    }
}

}