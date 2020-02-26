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

        int                     numberOfRows    = cardCount / cardsPerColumn;
        Dictionary<int, int>    classCount      = new Dictionary<int, int>();
        topRightCorner.x                       += (cardsPerRow - numberOfRows) / 2 * widthOffset;

        Vector3 spawnPosition                   = topRightCorner;

        for (int i = 0; i < cardCount / 2; i++)
        {
            classCount.Add(i, 0);
        }

        for (int i = 0; i < cardsPerColumn; i++)
        {
            spawnPosition.x = topRightCorner.x;

            for (int j = 0; j < numberOfRows; j++)
            {

                int classNumber = Random.Range(0, classCount.Count);

                while (classCount[classNumber] >= 2)
                {
                    classNumber = Random.Range(0, classCount.Count);
                }

                Instantiate(cardClasses[classNumber], spawnPosition, Quaternion.identity);

                classCount[classNumber]++;
                spawnPosition.x += widthOffset;
            }

            spawnPosition.y += heightOffset;
        }
    }
}

}