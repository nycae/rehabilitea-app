using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Memory;

namespace Memory.NPC
{

public class MemoryCard : MonoBehaviour
{
    public delegate void Selected();

    public event Selected OnSelect;

    [HideInInspector]
    public bool needsToTurn = false;

    [HideInInspector]
    private float beginTurnTime = 0.0f;

    [HideInInspector]
    private bool isHidden = true;

    [SerializeField, Range(0.1f, 5.0f)]
    private float timeToTurn = 0.5f;

    [HideInInspector]
    private static float degreesToTurn = 180f;

    public void Select()
    {
        if (isHidden)
        {
            TurnArround();
        }
    }

    public void TurnArround()
    {
        beginTurnTime = Time.time;
        needsToTurn = true;
        isHidden = !isHidden;
    }

    private void Update()
    {
        if (needsToTurn == true)
        {
            if (Time.time > beginTurnTime + timeToTurn)
            {
                needsToTurn = false;
                gameObject.transform.rotation = isHidden ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, degreesToTurn, 0);
            }
            else
            {
                gameObject.transform.Rotate(transform.up, Time.deltaTime * degreesToTurn / timeToTurn);
            }
        }
    }
}

}