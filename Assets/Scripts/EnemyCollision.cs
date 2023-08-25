using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Coroutine damageCoroutine;

    [SerializeField] private int laserDamage = 10; // Damage to take if hit by laser
    [SerializeField] private int shieldDamage = 30; // Damage to take if hit by shield

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy collided with: " + other.gameObject.name + " with layer: " + LayerMask.LayerToName(other.gameObject.layer));
        HandleCollision(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            damageCoroutine = StartCoroutine(ContinuousDamage(laserDamage));
        }
        // Add similar code for other continuous damage sources if needed
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
        // Add similar code for other continuous damage sources if needed
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Enemy collided with: " + other.gameObject.name + " with layer: " + LayerMask.LayerToName(other.gameObject.layer));
    }
    private void HandleCollision(Collider2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        switch (layerName)
        {
            case "Laser":
                Debug.Log("Laser Hit enemy" + other.gameObject.name);
                enemyHealth.TakeDamage(laserDamage);
                break;

            case "shield":
                
                enemyHealth.TakeDamage(shieldDamage);
                break;

            // ... any other cases you might want to handle

            default:
                // Default behavior, if any
                break;
        }
    }

    IEnumerator ContinuousDamage(int damageAmount)
    {
        while (true)
        {
            enemyHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(.1f);  // Wait for 1 second
        }
    }
}