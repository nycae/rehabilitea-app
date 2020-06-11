using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RehabiliTEA
{
    public class ClickableSprite : MonoBehaviour
    {
        private void Awake()
        {
            /* Make sure that the object has a sprite and a collider */
            Assert.IsNotNull(gameObject.GetComponent<Collider>());
            Assert.IsNotNull(gameObject.GetComponent<SpriteRenderer>());
        }

        public SpriteRenderer GetSpriteRenderer()
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }
}
