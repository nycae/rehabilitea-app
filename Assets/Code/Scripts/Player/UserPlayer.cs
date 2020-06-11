using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RehabiliTEA.Player
{
    public class UserPlayer : MonoBehaviour
    {
        private float               blockTimestamp;
        private bool                isBlocked = false;

        static public float         SecondsWhenBlocked = 1;
        public delegate void        Select(Sprite sprite);
        static public event Select  OnSelect;

        public void Block()
        {
            isBlocked       = true;
            blockTimestamp  = Time.time;
        }

        private void Update()
        {
            if (isBlocked)
            {
                if (Time.time >= blockTimestamp + SecondsWhenBlocked)
                {
                    isBlocked = false;
                }
            }
            else
            {
                if (Input.GetMouseButton(0)) CastRay();
            }
        }

        private void CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Sprite sprite = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite;

                if (sprite)
                {
                    OnSelect.Invoke(sprite);
                }
            }
        }
    }
}