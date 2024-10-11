using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;               // Speed at which the enemy moves towards the player
    public Color enemyColor;               // The current color of the enemy
    private Transform player;              // Reference to the player's transform
    private SpriteRenderer spriteRenderer; // Reference to the enemy's sprite renderer
    private bool isGameOver = false;       // Game Over state flag

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        AssignRandomColor(); // Assign a random color to the enemy on start
    }

    void Update()
    {
        if (!isGameOver)
        {
            MoveTowardsPlayer();  // Move towards the player
        }
    }

    // Method to move the enemy towards the player's position
    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    // Assign a random color to the enemy from a predefined array
    void AssignRandomColor()
    {
        Color[] availableColors = { Color.red, Color.yellow, Color.green, Color.blue };
        enemyColor = availableColors[Random.Range(0, availableColors.Length)];
        spriteRenderer.color = enemyColor;  // Update the enemy's sprite color to the assigned random color
    }

    // Public method to destroy the enemy
    public void DestroyEnemy()
    {
        Debug.Log("Enemy destroyed!");
        Destroy(gameObject);  // Destroy the enemy game object
    }

    // Handle collisions with the player and bullets
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameOver(); // Trigger game over if enemy reaches the player
        }
        else if (collision.CompareTag("Bullet"))
        {
            // Get the Bullet script/component attached to the bullet object
            Bullet bullet = collision.GetComponent<Bullet>();

            // If the bullet exists and its color matches the enemy's color, destroy the enemy
            if (bullet != null && bullet.bulletColor == enemyColor)
            {
                DestroyEnemy();  // Destroy the enemy if the bullet color matches the enemy's color
            }

            Destroy(collision.gameObject); // Destroy the bullet after it hits the enemy
        }
    }

    // Method to handle Game Over when the enemy reaches the player
    void GameOver()
    {
        Debug.Log("Game Over! Enemy reached the player.");
        isGameOver = true;  // Set the game over flag to prevent further movement
        Time.timeScale = 0f;  // Freeze the game (optional, depending on game logic)
        // You can optionally trigger a UI panel or Game Over screen here
    }
}











