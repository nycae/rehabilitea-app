using System.Collections;
using System.Collections.Generic;
using RehabiliTEA;
using UnityEngine;

namespace HideAndSeek
{
    public class HideAndSeekSpawnManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject      baseObject      = null;

        [SerializeField, Range(0.0f, 10.0f)]
        private float           minDistance     = 0.0f;

        [SerializeField]
        private float           upperBound      = 0.0f;

        [SerializeField]
        private float           lowerBound      = 0.0f;

        [SerializeField]
        private float           rightBound      = 0.0f;

        [SerializeField]
        private float           leftBound       = 0.0f;

        private List<Vector3>   spawnPositions  = new List<Vector3>();


        public GameObject Spawn(Sprite sprite)
        {
            Vector3     objectPosition  = GeneratePosition();
            GameObject  objectBuffer    = new GameObject("Sprite");

            spawnPositions.Add(objectPosition);

            objectBuffer.AddComponent<SpriteRenderer>();
            objectBuffer.GetComponent<SpriteRenderer>().sprite  = sprite;
            objectBuffer.transform.position                     = objectPosition;
            objectBuffer.transform.localScale                   = new Vector3(0.4f, 0.4f, 0.4f);

            return objectBuffer;
        }

        public GameObject SpawnClickable(Sprite sprite, Vector3 position)
        {
            GameObject objectBuffer = Instantiate(baseObject, position, Quaternion.identity);

            objectBuffer.GetComponent<ClickableSprite>().GetSpriteRenderer().sprite = sprite;

            return objectBuffer;
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
            Vector3 candidatePosition = new Vector3(Random.Range(leftBound, rightBound), Random.Range(lowerBound, upperBound), 0.0f);

            foreach (var position in spawnPositions)
                if (minDistance > Vector3.Distance(candidatePosition, position))
                    return GeneratePosition();

            return candidatePosition;
        }
    }
}
