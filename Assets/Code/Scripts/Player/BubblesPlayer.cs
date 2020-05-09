using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bubbles.Player
{

public class BubblesPlayer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                NPC.Bubble bubble = hit.collider.gameObject.GetComponent<NPC.Bubble>();

                if (bubble)
                {
                    bubble.Select();
                }
            }
        }
    }
}

}