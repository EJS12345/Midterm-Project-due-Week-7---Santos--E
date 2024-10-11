using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;               // Bullet speed
    public Color bulletColor;              // The color of the bullet
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer for the bullet color

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = bulletColor;  // Set the bullet color
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}













