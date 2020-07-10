using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace Memory
{
    public class MemoryPlayer : Player
    {
        void Start()
        {
            //SecondsWhenBlocked      = MemoryCard.TimeToTurn * 1.5f;
            FindObjectOfType<MemoryGameMode>().OnPair  += OnPair;
        }

        private void OnPair(bool wasSucessfull)
        {
            if (!wasSucessfull)
            {
                Block(MemoryCard.TimeToTurn * 1.5f);
            }
        }

        protected override void CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MemoryCard card = hit.collider.gameObject.GetComponent<MemoryCard>();

                if (card)
                {
                    card.Select();
                }
            }
        }
    }
}