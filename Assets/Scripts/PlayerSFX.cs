using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource powerUp;
    [SerializeField] private AudioSource playerDamage;
    [SerializeField] private AudioSource shieldCollision;
    [SerializeField] private AudioSource enemyDeath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayPowerUp()
    {
        powerUp.Play();
    }
    public void PlayerDamage()
    {
        playerDamage.Play();
    }
    public void ShieldCollision()
    {
        shieldCollision.Play();
    }
    public void EnemyKilled()
    {
        enemyDeath.Play();
    }
}
