using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace RehabiliTEA
{
    public class AssetManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] abecedary = new Sprite[0];
        [SerializeField] private Sprite[] numbers   = new Sprite[0];
        [SerializeField] private Sprite[] shapes    = new Sprite[0];
        [SerializeField] private Sprite[] drawings  = new Sprite[0];    

        public Sprite[] GetAbecedaryAssets()
        {
            return abecedary;
        }

        public Sprite[] GetNumberAssets()
        {
            return numbers;
        }

        public Sprite[] GetShapeAssets()
        {
            return shapes;
        }

        public Sprite[] GetRandomColorNumberSequence()
        {
            int         colorIndex      = Random.Range(1, 9);
            Sprite[]    selectedNumbers = new Sprite[10];

            for (int i = 0; i < numbers.Length; i += 9)
            {
                selectedNumbers[i / 9] = numbers[i + colorIndex];
            }

            return selectedNumbers;
        }

        public Sprite[] GetRandomColorAbecedarySequence()
        {
            int         colorIndex          = Random.Range(1, 9);
            Sprite[]    selectedAbecedary   = new Sprite[26];

            for (int i = 0; i < abecedary.Length; i += 9)
            {
                selectedAbecedary[i / 9] = abecedary[i + colorIndex];
            }

            return selectedAbecedary;
        }

        public Sprite[] GetRandomMixedColorNumberSequence()
        {
            int         colorIndex;
            Sprite[]    selectedNumbers = new Sprite[10];

            for (int i = 0; i < numbers.Length; i += 9)
            {
                colorIndex              = Random.Range(1, 9);
                selectedNumbers[i / 9]  = numbers[i + colorIndex];
            }

            return selectedNumbers;
        }

        public Sprite[] GetRandomShapes(int numberOfShapes)
        {
            if (numberOfShapes < 0 || numberOfShapes > shapes.Length) return null;

            HashSet<Sprite> resultSprite = new HashSet<Sprite>();

            while (resultSprite.Count < numberOfShapes)
            {
                resultSprite.Add(shapes[Random.Range(0, shapes.Length)]);
            }

            return Enumerable.ToArray(resultSprite);
        }

        public Sprite[] GetRandomShapesAndCharacters(int numberOfShapes)
        {
            if (numberOfShapes < 0 || numberOfShapes > abecedary.Length + numbers.Length + shapes.Length) return null;

            Sprite[]        allSprites  = abecedary.Concat(numbers).Concat(shapes).ToArray();
            HashSet<Sprite> sprites     = new HashSet<Sprite>();

            while (sprites.Count < numberOfShapes)
            {
                sprites.Add(allSprites[Random.Range(0, allSprites.Length)]);
            }

            return Enumerable.ToArray(sprites);   
        }

        public Sprite[] GetRandomSprites(int numberOfShapes)
        {
            if (numberOfShapes < 0 || numberOfShapes > abecedary.Length + numbers.Length + shapes.Length + drawings.Length) return null;

            Sprite[]        allSprites  = abecedary.Concat(numbers).Concat(shapes).Concat(drawings).ToArray();
            HashSet<Sprite> sprites     = new HashSet<Sprite>();
 
            while (sprites.Count < numberOfShapes)
            {
                sprites.Add(allSprites[Random.Range(0, allSprites.Length)]);
            }

            return System.Linq.Enumerable.ToArray(sprites);   
        }

        public Sprite[] GetRandomDrawings(int numberOfShapes)
        {
            if (numberOfShapes < 0 || numberOfShapes > drawings.Length) return null;

            HashSet<Sprite> resultSprite = new HashSet<Sprite>();

            while (resultSprite.Count < numberOfShapes)
            {
                resultSprite.Add(drawings[Random.Range(0, drawings.Length)]);
            }

            return resultSprite.ToArray();
        }
    }
}