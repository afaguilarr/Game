using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryBox : MonoBehaviour
{
    public bool empty = false;
    SpriteRenderer sprite_renderer;

    private void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }
}
