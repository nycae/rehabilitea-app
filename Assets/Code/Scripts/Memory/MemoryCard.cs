using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.NPC
{
    public class MemoryCard : MonoBehaviour
    {
        public delegate void    Selected(MemoryCard card);
        public event            Selected        OnSelect;

        [SerializeField]
        private GameObject      figure          = null;

        [HideInInspector]
        public bool             isTurning       = false;

        [Range(0.1f, 5.0f)]
        public static float     timeToTurn      = 0.5f;

        [HideInInspector]
        private float           beginTurnTime   = 0.0f;

        [SerializeField]
        private bool            isHidden        = true;

        [HideInInspector]
        private static float    degreesToTurn   = 180.0f;

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
            beginTurnTime = Time.time;
            isTurning = true;
            isHidden = !isHidden;
        }

        private void Update()
        {
            if (isTurning == true)
            {
                if (Time.time > beginTurnTime + timeToTurn)
                {
                    isTurning = false;
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

        public Sprite GetFigure()
        {
            return figure.GetComponent<SpriteRenderer>().sprite;
        }

        public void SetSprite(Sprite newSprite)
        {
            figure.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
}