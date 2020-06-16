using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.Player
{
    public class MemoryPlayer : MonoBehaviour
    {
        private bool isClickBlocked = false;

        void Start()
        {
            Framework.MemoryGameManager.OnPair += OnPair;
        }

        private void OnPair(bool wasSucessfull)
        {
            if (!wasSucessfull)
            {
                isClickBlocked = true;
                Invoke("UnblockMouse", NPC.MemoryCard.timeToTurn);
            }
        }

        private void UnblockMouse()
        {
            isClickBlocked = false;
        }

        void Update()
        {
            if (isClickBlocked) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray         ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit  hit;

                if (Physics.Raycast(ray, out hit))
                {
                    NPC.MemoryCard card = hit.collider.gameObject.GetComponent<NPC.MemoryCard>();

                    if (card != null)
                    {
                        card.Select();
                    }
                }
            }
        }
    }
}