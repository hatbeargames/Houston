using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Amount of damage to deal when colliding with an enemy.
    //[SerializeField] private int collisionDamage = 10;
    private PlayerStats playerStats;
    private PlayerMovement pm;
    private Coroutine damageCoroutine;
    private void Start()
    {
        // Get the PlayerStats component.
        playerStats = GetComponent<PlayerStats>();
        pm = GetComponent<PlayerMovement>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player collided with: " + other.gameObject.name + " with tag: " + other.tag);
        // Check if the colliding object has the enemy tag. You can change "Enemy" to whatever tag your enemy objects have.
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                if (!pm.GetShieldStatus())
                {
                    // Call the TakeDamage function from the PlayerStats script using the damage value from EnemyStats.
                    //playerStats.TakeDamage(enemyStats.damage);
                    damageCoroutine = StartCoroutine(ContinuousDamage(other));
                }
            }
            else
            {
                Debug.LogError("EnemyStats component not found on " + other.gameObject.name);
            }
        }
    }
    IEnumerator ContinuousDamage(Collider2D enemyCollider)
    {
        EnemyStats enemyStats = enemyCollider.GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats component not found on " + enemyCollider.gameObject.name);
            yield break;  // Exit the coroutine if EnemyStats is not found
        }

        while (true)
        {
            // Call the TakeDamage function from the PlayerStats script using the damage value from EnemyStats.
            playerStats.TakeDamage(enemyStats.damage);
            yield return new WaitForSeconds(1f);  // Wait for 1 second
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // If the player exits the enemy's trigger, stop the damage coroutine.
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;  // Reset the reference
            }
        }
    }
}
