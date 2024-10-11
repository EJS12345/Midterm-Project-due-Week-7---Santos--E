using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    public Color bulletColor;  // Color of the bullet

    void Start()
    {
        GetComponent<SpriteRenderer>().color = bulletColor;  // Set the initial color of the bullet
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && bulletColor == enemy.enemyColor)
            {
                enemy.DestroyEnemy();  // Destroy the enemy if the bullet color matches the enemy's color
            }
            Destroy(gameObject);  // Destroy the bullet after hitting an enemy
        }
    }
}


