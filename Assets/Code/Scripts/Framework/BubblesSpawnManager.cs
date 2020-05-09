using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global.Framework;

namespace Bubbles.Framework
{


public class BubblesSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject          bubbleObject    = null;

    [SerializeField]
    private Vector3             childScale      = new Vector3(0.5f, 0.5f, 0.5f);

    [SerializeField]
    private AssetManager        assetManager    = null;

    [SerializeField, Range(0.0f, 10.0f)]
    private float               minDistance     = 0.0f;

    [SerializeField]
    private float               upperBound      = 0.0f;

    [SerializeField]
    private float               lowerBound      = 0.0f;

    [SerializeField]
    private float               rightBound      = 0.0f;

    [SerializeField]
    private float               leftBound       = 0.0f;

    private GameObject[]        spawnedObjects  = null;

    private Sprite[]            spawnSequence   = null;

    private int                 currentSprite   = 0;

    public Sprite[] GenerateSpriteSequences(int numberOfBubbles, int numberOfDisctractions)
    {
        Sprite[] numberSequence         = assetManager.GetRandomColorNumberSequence();
        Sprite[] distractionSequence    = assetManager.GetRandomShapes(10);

        Sprite[] sequence               = new Sprite[numberOfBubbles];
        spawnSequence                   = new Sprite[numberOfBubbles + numberOfDisctractions];

        spawnedObjects                  = new GameObject[numberOfBubbles + numberOfDisctractions];

        for (int i = 0; i < numberOfDisctractions; i++)
        {
            spawnSequence[Random.Range(0, spawnSequence.Length - 1)] = distractionSequence[Random.Range(0, distractionSequence.Length - 1)];
        }

        for (int i = 0; i < sequence.Length; i++)
        {
            sequence[i] = numberSequence[i];
        }

        for (int i = 0; i < spawnSequence.Length; i++)
        {
            if (spawnSequence[i] == null)
            {
                spawnSequence[i] = numberSequence[currentSprite];
                currentSprite++;
            }
        }
        
        currentSprite = 0;

        return numberSequence;
    }

    public GameObject SpawnNextSprite()
    {
        Vector3     bubblePosition  = GeneratePosition();
        Vector3     contentOffset   = new Vector3(0.0f, 0.0f, 1.0f);
        GameObject  bubble          = Instantiate(bubbleObject);
        GameObject  content         = bubble.transform.GetChild(0).gameObject;

        bubble.transform.position                       = bubblePosition;
        content.transform.localScale                    = childScale;
        content.transform.localPosition                 = contentOffset;

        spawnedObjects[currentSprite]                   = bubble;

        bubble.GetComponent<NPC.Bubble>().SetSprite(spawnSequence[currentSprite]);

        currentSprite++;

        return bubble;
    }

    private Vector3 GeneratePosition()
    {
        Vector3 candidatePosition = new Vector3(Random.Range(leftBound, rightBound), Random.Range(lowerBound, upperBound), 0.0f);

        foreach (var sprite in spawnedObjects)
            if (sprite && minDistance > Vector3.Distance(sprite.transform.position, candidatePosition))
                return GeneratePosition();

        return candidatePosition;
    }

}


}