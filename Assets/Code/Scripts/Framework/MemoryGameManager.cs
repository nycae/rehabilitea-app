using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.Framework
{

public class MemoryGameManager : MonoBehaviour
{
    [SerializeField]
    public Vector3          topRightCorner;

    [SerializeField]
    public Vector3          bottomLeftCorner;

    [SerializeField]
    public NPC.MemoryCard[] cards;

    [SerializeField]
    public float            widthOffset;

    [SerializeField]
    public float            heightOffset;

    private int             numberOfCarts;

    void SpawnCards()
    {

    }
}   

}
