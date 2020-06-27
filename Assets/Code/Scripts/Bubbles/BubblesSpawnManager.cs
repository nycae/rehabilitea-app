using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace RehabiliTEA.Bubbles
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

        private List<GameObject>    spawnedObjects  = null;
        private Queue<Sprite>       spawnSequence   = null;

        public Sprite[] GenerateSpriteSequences(int numberOfBubbles, int numberOfDisctractions)
        {
            int             spritesToSpawnCount = numberOfBubbles + numberOfDisctractions;
            List<Sprite>    numberSequence      = assetManager.GetRandomColorNumberSequence().Take(numberOfBubbles).ToList();

            HashSet<int>    distractionIndices  = new HashSet<int>();
            Queue<Sprite>   numbersCopy         = new Queue<Sprite>(numberSequence);
            Queue<Sprite>   distractionSequence = new Queue<Sprite>(assetManager.GetRandomShapes(numberOfDisctractions + 1));

                            spawnSequence       = new Queue<Sprite>();
                            spawnedObjects      = new List<GameObject>();

            while (distractionIndices.Count < numberOfDisctractions)
            {
                distractionIndices.Add(Random.Range(0, spritesToSpawnCount));
            }

            for (int i = 0; i < spritesToSpawnCount; i++)
            {
                spawnSequence.Enqueue(distractionIndices.Contains(i) ? distractionSequence.Dequeue() : numbersCopy.Dequeue());
            }

            return numberSequence.ToArray();
        }

        public GameObject SpawnNextSprite()
        {
            Vector3     bubblePosition                      = GeneratePosition();
            Vector3     contentOffset                       = new Vector3(0.0f, 0.0f, 1.0f);
            GameObject  bubble                              = Instantiate(bubbleObject);
            GameObject  content                             = bubble.transform.GetChild(0).gameObject;

            bubble.transform.position                       = bubblePosition;
            bubble.GetComponent<SpriteRenderer>().sprite    = spawnSequence.Dequeue();
            content.transform.localPosition                 = contentOffset;

            spawnedObjects.Add(bubble);

            return bubble;
        }

        private Vector3 GeneratePosition()
        {
            Vector3 candidatePosition = new Vector3(Random.Range(leftBound, rightBound), Random.Range(lowerBound, upperBound), 0.0f);

            foreach (var sprite in spawnedObjects)
            {
                if (sprite && minDistance > Vector3.Distance(sprite.transform.position, candidatePosition))
                {
                    return GeneratePosition();
                }
            }

            return candidatePosition;
        }
    }
}