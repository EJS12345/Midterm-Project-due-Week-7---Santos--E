using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab of the enemy to spawn

    // Spawn area corners
    public Transform spawnAreaTopLeft;
    public Transform spawnAreaTopRight;
    public Transform spawnAreaBottomLeft;
    public Transform spawnAreaBottomRight;

    // Spawn settings
    public float spawnInterval = 2.0f; // Time between spawns
    public int maxEnemies = 10; // Max number of enemies at once
    private int currentEnemyCount = 0;

    public Transform playerTransform; // Reference to the player

    void Start()
    {
        if (!AreSpawnAreaTransformsAssigned())
        {
            Debug.LogError("One or more spawn area transforms are not assigned.");
            return; // Exit Start if the transforms are not set
        }

        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (currentEnemyCount < maxEnemies) // Only spawn while enemy count is below the max
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemyCount < maxEnemies)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition();
                if (IsPositionValid(spawnPosition)) // Ensure the position is valid
                {
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    currentEnemyCount++;
                }
            }
        }

        Debug.Log("Maximum enemy count reached. Stopping spawner.");
    }

    Vector2 GetRandomSpawnPosition()
    {
        if (!AreSpawnAreaTransformsAssigned())
        {
            return Vector2.zero; // Return a default position or handle it as needed
        }

        float minX = Mathf.Min(spawnAreaTopLeft.position.x, spawnAreaBottomLeft.position.x);
        float maxX = Mathf.Max(spawnAreaTopRight.position.x, spawnAreaBottomRight.position.x);
        float minY = Mathf.Min(spawnAreaBottomLeft.position.y, spawnAreaBottomRight.position.y);
        float maxY = Mathf.Max(spawnAreaTopLeft.position.y, spawnAreaTopRight.position.y);

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector2(x, y);
    }

    public void EnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1); // Ensure it doesn't go negative
    }

    private bool AreSpawnAreaTransformsAssigned()
    {
        return spawnAreaTopLeft != null && spawnAreaTopRight != null &&
               spawnAreaBottomLeft != null && spawnAreaBottomRight != null &&
               playerTransform != null; // Ensure playerTransform is assigned
    }

    private bool IsPositionValid(Vector2 position)
    {
        // Check if the spawn position is too close to the player
        float distanceToPlayer = Vector2.Distance(position, playerTransform.position);
        return distanceToPlayer > 1.0f; // Adjust this threshold as needed
    }

    private void OnValidate()
    {
        if (!AreSpawnAreaTransformsAssigned())
        {
            Debug.LogWarning("One or more spawn area transforms are not assigned. Please assign all four corners in the Inspector.");
        }
    }
}











