using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace RehabiliTEA
{
    public class AssetManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] abecedary  = Array.Empty<Sprite>();
        [SerializeField] private Sprite[] numbers    = Array.Empty<Sprite>();
        [SerializeField] private Sprite[] shapes     = Array.Empty<Sprite>();
        [SerializeField] private Sprite[] drawings   = Array.Empty<Sprite>();    
        private const            int      ColorCount = 9;

        public Sprite[] GetRandomColorNumberSequence()
        {
            var         colorIndex      = Random.Range(1, ColorCount);
            var    selectedNumbers = new Sprite[numbers.Length / ColorCount];

            for (var i = 0; i < numbers.Length; i += ColorCount)
            {
                selectedNumbers[i / ColorCount] = numbers[i + colorIndex];
            }

            return selectedNumbers;
        }

        public Sprite[] GetRandomColorAbecedarySequence()
        {
            var colorIndex        = Random.Range(1, ColorCount);
            var selectedAbecedary = new Sprite[abecedary.Length / ColorCount];

            for (var i = 0; i < abecedary.Length; i += ColorCount)
            {
                selectedAbecedary[i / ColorCount] = abecedary[i + colorIndex];
            }

            return selectedAbecedary;
        }

        public Sprite[] GetRandomShapes(int numberOfShapes)
        {
            if (numberOfShapes < 0 || numberOfShapes > shapes.Length) return null;

            var resultSprite = new HashSet<Sprite>();

            while (resultSprite.Count < numberOfShapes) 
                resultSprite.Add(shapes[Random.Range(0, shapes.Length)]);

            return resultSprite.ToArray();
        }

        public Sprite[] GetRandomShapesAndCharacters(int numberOfShapes)
        {
            if (numberOfShapes < 0 ||
                numberOfShapes > abecedary.Length + numbers.Length + shapes.Length) return null;

            var allSprites = abecedary.Concat(numbers).Concat(shapes).ToArray();
            var sprites    = new HashSet<Sprite>();

            while (sprites.Count < numberOfShapes)
                sprites.Add(allSprites[Random.Range(0, allSprites.Length)]);

            return sprites.ToArray();   
        }

        public Sprite[] GetRandomSprites(int numberOfShapes)
        {
            if (numberOfShapes >
                abecedary.Length + numbers.Length + shapes.Length + drawings.Length ||
                numberOfShapes < 0) return null;

            var allSprites = abecedary.Concat(numbers).Concat(shapes).Concat(drawings).ToArray();
            var sprites    = new HashSet<Sprite>();

            while (sprites.Count < numberOfShapes)
                sprites.Add(allSprites[Random.Range(0, allSprites.Length)]);

            return sprites.ToArray();   
        }

        public Sprite[] GetRandomDrawings(int numberOfShapes)
        {
            if (numberOfShapes < 0 || numberOfShapes > drawings.Length) return null;

            var resultSprite = new HashSet<Sprite>();

            while (resultSprite.Count < numberOfShapes)
                resultSprite.Add(drawings[Random.Range(0, drawings.Length)]);

            return resultSprite.ToArray();
        }
    }
}