using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] StatsBarScript healthBar;
    [SerializeField] ChanceToSpawn cts;
    private PlayerCollision pc;
    [SerializeField] private float minChance;
    [SerializeField] private float maxChance;
    private float spawnChance;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthBar.SetMaxBarValue(startingHealth);
        spawnChance = Random.Range(minChance, maxChance);
        cts = GetComponent<ChanceToSpawn>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthBar.SetBarValue(currentHealth);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            var enemyStats = GetComponent<EnemyStats>();
            if (enemyStats != null && enemyStats.DamageCoroutine != null)
            {
                pc.StopSpecificCoroutine(enemyStats.DamageCoroutine);
            }
            if (cts)
            {
                cts.SpawnRandomObject(spawnChance);
            }
            Destroy(this.gameObject);
        }
    }
}
