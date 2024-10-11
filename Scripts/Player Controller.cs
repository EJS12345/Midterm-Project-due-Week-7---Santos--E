using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;    // Prefab of the bullet
    public Transform firePoint;        // Point where bullets are fired from
    public float bulletForce = 10f;    // Speed of the bullets
    public float attackRange = 5f;     // Range within which the player can detect and attack enemies
    public LayerMask enemyLayer;       // LayerMask to specify which objects are considered enemies
    public float shootCooldown = 1f;   // Cooldown time between shots
    public float rotationSpeed = 2f;   // Speed of rotation towards target

    // Define 4 specific colors: Blue, Red, Yellow, Green
    private Color[] availableColors = { Color.red, Color.yellow, Color.green, Color.blue };
    private Color playerColor;         // Current color of the player and bullet

    private EnemyController targetEnemy;  // The currently targeted enemy
    private bool enemyInRange = false;    // Flag to track if the enemy has entered the range
    private bool canShoot = true;         // Flag to manage shooting cooldown

    void Start()
    {
        ChangeColor(availableColors[Random.Range(0, availableColors.Length)]); // Set a random initial color
    }

    void Update()
    {
        // Continuously detect enemies inside the barrier/attack range
        DetectEnemyInRange();

        // If an enemy is in range, rotate towards it and shoot if possible
        if (enemyInRange && targetEnemy != null)
        {
            RotateTowardsEnemy(targetEnemy.transform.position);  // Rotate towards the enemy

            // Check if the player can shoot
            if (canShoot)
            {
                // Change color to match the enemy right before shooting
                ChangeColor(targetEnemy.enemyColor);
                ShootBulletAtTarget();  // Shoot at the target enemy
            }
        }
    }

    // Change the player color to the specified color
    void ChangeColor(Color newColor)
    {
        playerColor = newColor;  // Set player's color
        GetComponent<SpriteRenderer>().color = playerColor;  // Update the player's color visually
    }

    // Detect the closest enemy within the attack range (barrier) and set it as a target
    void DetectEnemyInRange()
    {
        // If there's already a target and it's still in range, skip detection
        if (targetEnemy != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetEnemy.transform.position);
            if (distanceToTarget <= attackRange)
            {
                return;  // Keep the current target if it's still in range
            }
        }

        // Use Physics2D.OverlapCircle to find all colliders within the attack range (barrier)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        targetEnemy = null; // Reset the target
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyController enemy = hitCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);

                    // Check if this enemy is closer than the previously found enemy
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetEnemy = enemy;   // Set this enemy as the target
                        enemyInRange = true;   // Mark that an enemy is in range
                    }
                }
            }
        }

        // If no enemy is found in range, stop attacking
        if (targetEnemy == null)
        {
            enemyInRange = false;
        }
    }

    // Rotate the player towards the targeted enemy
    void RotateTowardsEnemy(Vector3 targetPosition)
    {
        // Calculate direction towards enemy
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Determine rotation angle to the enemy
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Subtract 90 to match top of sprite
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Smoothly rotate towards enemy
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    // Shoot bullets automatically at the targeted enemy
    void ShootBulletAtTarget()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("BulletPrefab or FirePoint is not assigned in the Inspector");
            return;  // Exit if any required reference is missing
        }

        // Instantiate the bullet at the firePoint's position and set its properties
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<SpriteRenderer>().color = playerColor;  // Set bullet color
        bullet.GetComponent<BulletCollider>().bulletColor = playerColor;   // Pass color to bullet script

        // Calculate the direction toward the enemy's current position
        Vector2 direction = ((Vector2)targetEnemy.transform.position - (Vector2)firePoint.position).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletForce;  // Apply velocity toward the enemy

        Debug.DrawLine(firePoint.position, targetEnemy.transform.position, Color.green, 1f); // Draw debug line
        Debug.Log("Shooting at direction: " + direction);  // Debug the direction of the bullet

        // Start the cooldown coroutine to limit firing rate
        StartCoroutine(ShootingCooldown());
    }

    // Coroutine to handle shooting cooldown
    private IEnumerator ShootingCooldown()
    {
        canShoot = false;  // Prevent shooting
        yield return new WaitForSeconds(shootCooldown);  // Wait for the cooldown time
        canShoot = true;  // Allow shooting again
    }

    // Visualize the attack range in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);  // Draw a red circle to visualize the attack range
    }
}





























