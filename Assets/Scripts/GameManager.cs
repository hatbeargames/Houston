using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject player;
    [SerializeField] private bool GG = false;
    [SerializeField] private GameObject healthBarCritical;
    [SerializeField] private GameObject thrusterBarCritical;
    [SerializeField] private GameObject energyBarCritical;
    [SerializeField] private float WarningThreshold = .25f;

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private float playerSpeed;
    private float distanceToGoal;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!GG)
        {
            GameOverCheck();
        }
        PlayerStatCheck();
        Debug.Log("Speed" + playerSpeed + ", Altitude" + distanceToGoal);
    }

    void GameOverCheck()
    {
        playerMovement.enabled = !playerStats.isDead;
        GameOverScreen.SetActive(playerStats.isDead);
        GG = playerStats.isDead;
    }

    void PlayerStatCheck()
    {
        energyBarCritical.SetActive(playerStats.currentEnergy <= (playerStats.GetMaxEnergy() * WarningThreshold));
        healthBarCritical.SetActive(playerStats.currentHealth <= (playerStats.GetMaxHealth() * WarningThreshold));
        thrusterBarCritical.SetActive(playerStats.currentThrusters <= (playerStats.GetMaxThrust() * WarningThreshold));
    }
    public void SetSpeed(int sped)
    {
        playerSpeed = sped;
    }
    public void SetDistanceToGoal(int dist)
    {
        distanceToGoal = dist;
    }
}
