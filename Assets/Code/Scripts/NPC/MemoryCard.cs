using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.NPC
{

public class MemoryCard : MonoBehaviour
{
    public enum Figure
    {
        RedAce,
        BlackAce,
        RedTwo,
        BlackTwo,
        RedThree,
        BlackThree,
        RedFour,
        BlackFour,
        RedFive,
        BlackFive,
        RedSix,
        BlackSix,
        RedSeven,
        BlackSeven,
        RedEight,
        BlackEigh,
        MAX
    }

    public delegate void    Selected(MemoryCard card);

    public event Selected   OnSelect;

    [SerializeField]
    public Figure           figure;

    [HideInInspector]
    public bool             needsToTurn     = false;

    [Range(0.1f, 5.0f)]
    public static float    timeToTurn      = 0.5f;

    [HideInInspector]
    private float           beginTurnTime   = 0.0f;

    [SerializeField]
    private bool            isHidden        = true;

    [HideInInspector]
    private static float    degreesToTurn   = 180f;

    public void Select()
    {
        if (isHidden)
        {
            TurnArround();
            OnSelect.Invoke(this);
        }
    }

    public void TurnArround()
    {
        beginTurnTime   = Time.time;
        needsToTurn     = true;
        isHidden        = !isHidden;
    }

    private void Update()
    {
        if (needsToTurn == true)
        {
            if (Time.time > beginTurnTime + timeToTurn)
            {
                needsToTurn = false;
                gameObject.transform.rotation = isHidden 
                    ? Quaternion.Euler(0, 0, 0) 
                    : Quaternion.Euler(0, degreesToTurn, 0);
            }
            else
            {
                gameObject.transform.Rotate(transform.up, Time.deltaTime * degreesToTurn / timeToTurn);
            }
        }
    }

}

}