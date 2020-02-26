using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Memory.Player
{

public class MemoryPlayer : MonoBehaviour
{
    private int cardCount = 0;

    private bool isClickBlocked = false;

    void Start()
    {
        foreach (var card in FindObjectsOfType<NPC.MemoryCard>())
        {
            card.OnSelect += OnCardSelected;
        }
    }

    void OnCardSelected(NPC.MemoryCard card)
    {
        cardCount++;

        if (cardCount >= 2)
        {
            isClickBlocked = true;

            Invoke("UnblockMouse", NPC.MemoryCard.timeToTurn * 1.5f);
        }
    }


    private void UnblockMouse()
    {
        isClickBlocked  = false;
        cardCount       = 0;
    }

    void Update()
    {
        if (isClickBlocked)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

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