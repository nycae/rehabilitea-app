using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RehabiliTEA
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float           cadency             = 10f;
        private bool            isBlocked           = false;

        public delegate void    Select(GameObject sprite);
        public event Select     OnSelect;

        public void Block()
        {
            isBlocked = true;
        }
        
        public void Block(float timeBlocked)
        {
            isBlocked = true;
            Invoke("Free", timeBlocked);
        }

        public void Free()
        {
            isBlocked = false;
        }

        private void Update()
        {
            if (isBlocked)  
            {
                return;
            }
            else
            {
                if (Input.GetMouseButton(0)) CastRay();
            }
        }

        protected virtual void CastRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SpriteRenderer sprite = hit.collider.gameObject.GetComponent<SpriteRenderer>();

                if (sprite)
                {
                    OnSelect.Invoke(hit.collider.gameObject);
                    Block();
                    Invoke("Free", 1/cadency);
                }
            }
        }
    }
}