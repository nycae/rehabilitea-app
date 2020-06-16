using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace Memory
{
    public class MemoryCard : ClickableSprite
    {
        private bool            isTurning       = false;
        private bool            isHidden        = true;
        private float           beginTurnTime   = 0.0f;
        public static float     TimeToTurn      = 0.5f;
        private static float    DegreesToTurn   = 180.0f;

        public delegate void    Selected(MemoryCard card);
        public event Selected   OnSelect;

        public void Select()
        {
            if (isHidden && !isTurning)
            {
                TurnArround();
                OnSelect.Invoke(this);
            }
        }

        public void TurnArround()
        {
            beginTurnTime   = Time.time;
            isTurning       = true;
            isHidden        = !isHidden;
        }

        private void Update()
        {
            if (isTurning == true)
            {
                if (Time.time > beginTurnTime + TimeToTurn)
                {
                    isTurning = false;
                    gameObject.transform.rotation = isHidden
                        ? Quaternion.Euler(0f,  0f,     0f)
                        : Quaternion.Euler(0f,  180f,   0f);
                }
                else
                {
                    gameObject.transform.Rotate(transform.up, Time.deltaTime * DegreesToTurn / TimeToTurn);
                }
            }
        }
    }
}