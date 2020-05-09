using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bubbles.NPC
{

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer  spriteRenderer          = null;
    public delegate void    Selected(Bubble bubble);

    public event Selected   OnSelect;

    public void Select()
    {
        OnSelect.Invoke(this);
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public void SetSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

}

}