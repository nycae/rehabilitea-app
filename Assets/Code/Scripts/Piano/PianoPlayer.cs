using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;

namespace Piano
{
    public class PianoPlayer : Player
    {
        protected override void CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();

                if (tile)
                {
                    tile.Play();
                }
            }
        }
    }
}