using UnityEngine;
using RehabiliTEA;

namespace Memory
{
    public class MemoryCard : ClickableSprite
    {
        private       bool  isTurning;
        private       float beginTurnTime;
        private       bool  isHidden      = true;
        public const  float TimeToTurn    = 0.5f;
        private const float DegreesToTurn = 180.0f;

        public delegate void    Selected(MemoryCard card);
        public event Selected   OnSelect;

        public void Select()
        {
            if (!isHidden || isTurning) return;
            
            TurnAround();
            OnSelect?.Invoke(this);
        }

        public void TurnAround()
        {
            beginTurnTime   = Time.time;
            isTurning       = true;
            isHidden        = !isHidden;
        }

        private void Update()
        {
            if (!isTurning) return;
            
            if (Time.time > beginTurnTime + TimeToTurn)
            {
                isTurning = false;
                gameObject.transform.rotation = isHidden
                    ? Quaternion.Euler(0f, 0f,   0f)
                    : Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(transform.up, Time.deltaTime * DegreesToTurn / TimeToTurn);
            }
        }
    }
}