using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RehabiliTEA;
using Debug = System.Diagnostics.Debug;

namespace Piano
{
    public class PianoPlayer : Player
    {
        protected override void CastRay()
        {
            Debug.Assert(Camera.main != null, "Camera.main != null");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            var tile = hit.collider.gameObject.GetComponent<Tile>();

            if (!tile) return;
            tile.Play();
        }
    }
}