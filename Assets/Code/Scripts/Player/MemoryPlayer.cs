using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memory.Player
{

public class MemoryPlayer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                NPC.MemoryCard card = hit.collider.gameObject.GetComponent<NPC.MemoryCard>();

                if (card != null)
                {
                    card.OnSelect();
                }
            }
        }
    }
}


}