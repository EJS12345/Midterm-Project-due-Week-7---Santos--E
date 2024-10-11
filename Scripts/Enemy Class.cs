using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;            // Speed at which the enemy moves towards the player
    public Color enemyColor;            // The color of the enemy (Red, Yellow, Green, Blue)

    private Transform player;           // Reference to the player's transform

    void Start()
    {
        // Find the player object by its tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set a random color to the enemy (Red, Yellow, Green, Blue)
        enemyColor = GetRandomColor();
        GetComponent<SpriteRenderer>().color = enemyColor;
    }

    void Update()
    {
        MoveTowardsPlayer();  // Move towards the player every frame
    }

    // Function to make the enemy move towards the player
    void MoveTowardsPlayer()
    {
        // If the player exists, move towards its position
        if (player != null)
        {
            // Calculate the direction vector from the enemy to the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Move the enemy towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    // Function to return a random color from the four available colors
    Color GetRandomColor()
    {
        Color[] availableColors = { Color.red, Color.yellow, Color.green, Color.blue };
        return availableColors[Random.Range(0, availableColors.Length)];
    }

    // Function to handle enemy destruction
    public void DestroyEnemy()
    {
        // Add any effects here (explosion, sound, etc.)
        Destroy(gameObject);
    }
}


