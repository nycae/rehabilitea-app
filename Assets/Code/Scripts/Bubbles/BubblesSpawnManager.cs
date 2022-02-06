using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace RehabiliTEA.Bubbles
{
    public class BubblesSpawnManager : MonoBehaviour
    {
        [SerializeField, Range(0.0f, 10.0f)]
        private float minDistance;
        [SerializeField] private GameObject   bubbleObject;
        [SerializeField] private AssetManager assetManager;
        [SerializeField] private float        upperBound;
        [SerializeField] private float        lowerBound;
        [SerializeField] private float        rightBound;
        [SerializeField] private float        leftBound;

        private List<GameObject> spawnedObjects = new List<GameObject>();
        private Queue<Sprite>    spawnSequence  = new Queue<Sprite>();

        public Sprite[] GenerateSpriteSequences(int numberOfBubbles, int numberOfDistractions)
        {
            var spritesToSpawnCount = numberOfBubbles + numberOfDistractions;
            var numberSequence = assetManager.GetRandomColorNumberSequence().Take(numberOfBubbles)
                .ToList();

            var distractionIndices = new HashSet<int>();
            var numbersCopy        = new Queue<Sprite>(numberSequence);
            var distractionSequence =
                new Queue<Sprite>(assetManager.GetRandomShapes(numberOfDistractions + 1));

            spawnSequence  = new Queue<Sprite>();
            spawnedObjects = new List<GameObject>();

            while (distractionIndices.Count < numberOfDistractions)
                distractionIndices.Add(Random.Range(0, spritesToSpawnCount));

            for (var i = 0; i < spritesToSpawnCount; i++)
                spawnSequence.Enqueue(distractionIndices.Contains(i) ? distractionSequence.Dequeue() : numbersCopy.Dequeue());

            return numberSequence.ToArray();
        }

        public IEnumerable<Sprite> GenerateSpriteSequence(Difficulty difficulty, int numberOfDistractions)
        {
            Sprite[] targetSequence;

            var nextSpriteIndex      = 0;
            var nextDistractionIndex = 0;
            var distractions         = assetManager.GetRandomShapes(numberOfDistractions + 1);
            var distractionIndices   = new HashSet<int>();

            switch (difficulty)
            {
                case Difficulty.Easy:
                    targetSequence = assetManager.GetRandomColorNumberSequence();
                    break;

                case Difficulty.Medium:
                    targetSequence = assetManager.GetRandomColorAbecedarySequence();
                    break;

                default:
                    var numbers     = assetManager.GetRandomColorNumberSequence().Take(11).ToList();
                    var characters  = assetManager.GetRandomColorAbecedarySequence().Take(11).ToList();

                    targetSequence  = new Sprite[20];

                    for (int i = 0; i < 20; i++)
                    {   
                        if (i % 2 == 0) // even
                        {
                            targetSequence[i] = numbers[0];
                            numbers.RemoveAt(0);
                        }
                        else            // odd
                        {
                            targetSequence[i] = characters[0];
                            characters.RemoveAt(0);
                        }
                    }
                    
                    break;
            }

            var spawnCount = targetSequence.Length + numberOfDistractions;

            while (distractionIndices.Count < numberOfDistractions) 
                distractionIndices.Add(Random.Range(0, spawnCount));

            for (var i = 0; i < spawnCount; i++)
            {
                if (distractionIndices.Contains(i))
                {
                    spawnSequence.Enqueue(distractions[nextDistractionIndex]);
                    nextDistractionIndex++;
                }
                else
                {
                    spawnSequence.Enqueue(targetSequence[nextSpriteIndex]);
                    nextSpriteIndex++;
                }
            }

            return targetSequence;
        }

        public GameObject SpawnNextSprite()
        {
            var bubblePosition = GeneratePosition();
            var contentOffset  = new Vector3(0.0f, 0.0f, 1.0f);
            var bubble         = Instantiate(bubbleObject);
            var content        = bubble.transform.GetChild(0).gameObject;

            bubble.transform.position                    = bubblePosition;
            bubble.GetComponent<SpriteRenderer>().sprite = spawnSequence.Dequeue();
            content.transform.localPosition              = contentOffset;

            spawnedObjects.Add(bubble);

            return bubble;
        }

        private Vector3 GeneratePosition()
        {
            while (true)
            {
                var candidatePosition = new Vector3(Random.Range(leftBound, rightBound),
                    Random.Range(lowerBound,                                upperBound), 0.0f);

                if (spawnedObjects.Any(sprite =>
                    sprite && minDistance >
                    Vector3.Distance(sprite.transform.position, candidatePosition))) continue;
                return candidatePosition;
            }
        }
    }
}