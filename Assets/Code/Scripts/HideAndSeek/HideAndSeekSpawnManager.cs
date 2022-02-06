using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RehabiliTEA;
using UnityEngine;

namespace HideAndSeek
{
    public class HideAndSeekSpawnManager : MonoBehaviour
    {
        [SerializeField]                     private GameObject baseObject;
        [SerializeField, Range(0.0f, 10.0f)] private float      minDistance;
        [SerializeField]                     private float      upperBound;
        [SerializeField]                     private float      lowerBound;
        [SerializeField]                     private float      rightBound;
        [SerializeField]                     private float      leftBound;
        
        private readonly List<Vector3> spawnPositions = new List<Vector3>();


        public void Spawn(Sprite sprite)
        {
            var objectPosition = GeneratePosition();
            var objectBuffer   = new GameObject("Sprite");

            spawnPositions.Add(objectPosition);

            objectBuffer.AddComponent<SpriteRenderer>();
            objectBuffer.GetComponent<SpriteRenderer>().sprite = sprite;
            objectBuffer.transform.position                    = objectPosition;
            objectBuffer.transform.localScale                  = new Vector3(0.4f, 0.4f, 0.4f);
        }

        public void SpawnClickable(Sprite sprite, Vector3 position)
        {
            var objectBuffer = Instantiate(baseObject, position, Quaternion.identity);

            objectBuffer.GetComponent<ClickableSprite>().GetSpriteRenderer().sprite = sprite;
        }

        public void DestroyAll()
        {
            foreach (var sprite in FindObjectsOfType<SpriteRenderer>())
            {
                Destroy(sprite.gameObject);
            }

            spawnPositions.Clear();
        }

        private Vector3 GeneratePosition()
        {
            while (true)
            {
                var candidatePosition = new Vector3(Random.Range(leftBound, rightBound), Random.Range(lowerBound, upperBound), 0.0f);

                if (spawnPositions.Any(position => minDistance > Vector3.Distance(candidatePosition, position)))
                {
                    continue;
                }

                return candidatePosition;
            }
        }
    }
}
