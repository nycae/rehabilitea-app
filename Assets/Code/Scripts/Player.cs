using UnityEngine;

namespace RehabiliTEA
{
    public class Player : MonoBehaviour
    {
        private const float     Cadence   = 10f;
        private       bool      isBlocked = false;
        
        public delegate void    Select(GameObject sprite);
        public event Select     OnSelect;

        private void Block()
        {
            isBlocked = true;
        }
        
        public void Block(float timeBlocked)
        {
            isBlocked = true;
            Invoke(nameof(Free), timeBlocked);
        }

        public void Free()
        {
            isBlocked = false;
        }

        private void Update()
        {
            if (isBlocked) return;
            if (Input.GetMouseButton(0)) CastRay();
        }

        protected virtual void CastRay()
        {
            if (!(Camera.main is null))
            {
                var        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit)) return;
                var sprite = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                if (!sprite) return;
                OnSelect?.Invoke(hit.collider.gameObject);
            }
            Block();
            Invoke(nameof(Free), 1/Cadence);
        }
    }
}