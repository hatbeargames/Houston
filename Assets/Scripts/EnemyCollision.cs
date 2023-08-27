using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Coroutine damageCoroutine;
    private PlayerSFX pSFX;

    [SerializeField] private int laserDamage = 10; // Damage to take if hit by laser
    [SerializeField] private int shieldDamage = 30; // Damage to take if hit by shield

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        pSFX = FindAnyObjectByType<PlayerSFX>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Enemy collided with: " + other.gameObject.name + " with layer: " + LayerMask.LayerToName(other.gameObject.layer));
        HandleCollision(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            damageCoroutine = StartCoroutine(ContinuousDamage(laserDamage));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("shield"))
        {
            damageCoroutine = StartCoroutine(ContinuousDamage(shieldDamage));
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
        if (other.gameObject.layer == LayerMask.NameToLayer("shield") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
        // Add similar code for other continuous damage sources if needed
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollision(other.collider);
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            damageCoroutine = StartCoroutine(ContinuousDamage(laserDamage));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("shield"))
        {
            damageCoroutine = StartCoroutine(ContinuousDamage(shieldDamage));
        }
        //Debug.Log("Enemy collided with: " + other.gameObject.name + " with layer: " + LayerMask.LayerToName(other.gameObject.layer));
    }
    private void HandleCollision(Collider2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);
        switch (layerName)
        {
            case "Laser":
                //Debug.Log("Laser Hit enemy" + other.gameObject.name);
                enemyHealth.TakeDamage(laserDamage);
                break;

            case "shield":
                pSFX.ShieldCollision();
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
            yield return new WaitForSeconds(.01f);  // Wait for 1 second
        }
    }
}
