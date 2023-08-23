using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    [SerializeField] private int laserDamage = 10; // Damage to take if hit by laser
    [SerializeField] private int shieldDamage = 30; // Damage to take if hit by shield

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //string layerName = LayerMask.LayerToName(other.gameObject.layer);

        //switch (layerName)
        //{
        //    case "laser":
        //        enemyHealth.TakeDamage(laserDamage);
        //        break;

        //    case "shield":
        //        enemyHealth.TakeDamage(shieldDamage);
        //        break;

        //    // ... any other cases you might want to handle

        //    default:
        //        // Default behavior, if any
        //        break;
        //}
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        switch (layerName)
        {
            case "Laser":
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
}
