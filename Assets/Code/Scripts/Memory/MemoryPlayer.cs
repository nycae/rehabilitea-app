using UnityEngine;
using RehabiliTEA;

namespace Memory
{
    public class MemoryPlayer : Player
    {
        private void Start()
        {
            //SecondsWhenBlocked      = MemoryCard.TimeToTurn * 1.5f;
            FindObjectOfType<MemoryGameMode>().OnPair  += OnPair;
        }

        private void OnPair(bool wasSuccessful)
        {
            if (!wasSuccessful) Block(MemoryCard.TimeToTurn * 1.5f);
        }

        protected override void CastRay()
        {
            if (Camera.main is null) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            var card = hit.collider.gameObject.GetComponent<MemoryCard>();
            
            if (card) card.Select();
        }
    }
}