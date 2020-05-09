using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Global.Framework
{


public class AssetManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] abecedary  = new Sprite[0];

    [SerializeField]
    private Sprite[] numbers    = new Sprite[0];

    [SerializeField]
    private Sprite[] shapes     = new Sprite[0];

    public Sprite[] GetAbecedaryAssets()
    {
        return abecedary;
    }

    public Sprite[] GetNumberAssets()
    {
        return numbers;
    }

    public Sprite[] GetShapesOfTheGame()
    {
        return shapes;
    }

    public Sprite[] GetRandomColorNumberSequence()
    {
        int         colorIndex      = Random.Range(1, 10);
        Sprite[]    selectedNumbers = new Sprite[10];

        for (int i = 0; i < numbers.Length; i += 10)
        {
            selectedNumbers[i/10] = numbers[i+colorIndex];
        }

        return selectedNumbers;
    }

    public Sprite[] GetRandomMixedColorNumberSequence()
    {
        int         colorIndex;
        Sprite[]    selectedNumbers    = new Sprite[10];

        for (int i = 0; i < numbers.Length; i += 10)
        {
            colorIndex              = Random.Range(1, 10);
            selectedNumbers[i/10]   = numbers[i+colorIndex];
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

        return System.Linq.Enumerable.ToArray(resultSprite);
    }

}


}