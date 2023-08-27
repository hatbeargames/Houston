using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject player;
    [SerializeField] private bool GG = false;
    [SerializeField] private GameObject healthBarCritical;
    [SerializeField] private GameObject thrusterBarCritical;
    [SerializeField] private GameObject energyBarCritical;
    [SerializeField] private float WarningThreshold = .25f;
    [SerializeField] private GameObject tutorialZone;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject spawnPoints;
    [SerializeField] private GameObject burnUp;
    [SerializeField] private GameObject burnUp2;
    [SerializeField] private GameObject burnUp3;
    [SerializeField] private GameObject controlsUI;
    [SerializeField] private GameObject HUDUI;
    [SerializeField] private Material normalMaterial1;
    [SerializeField] private Material slowDownMaterial1;
    [SerializeField] private Material FastMaterial1;
    [SerializeField] private Material burnUpMaterial2;
    [SerializeField] private TMP_Text finalScore;
    public TMP_Text myTextObject;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private float playerSpeed;
    private float TooFastThreshold = 140;
    private float slowDownThreshold;
    private float FastThreshold;
    private float distanceToGoal = 20000;
    private bool tutorialCleared = false;
    private bool spawnEnabled = false;
    public int BurnDamage;
    private bool burningUp;
    private TopScoreTracker tst;
    void Start()
    {
        tst = FindAnyObjectByType<TopScoreTracker>();
        slowDownThreshold = TooFastThreshold * .50f;
        FastThreshold = TooFastThreshold * .75f;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        CheckSpeed();
        burnUp.SetActive(burningUp);
        burnUp2.SetActive(burningUp);
        burnUp3.SetActive(burningUp);
        SetHUDColor();
        if (!playerStats.isDead)
        {
            SetTopScore();
        }
        if (burningUp && !IsInvoking("DealBurningDamage"))
        {
            
            InvokeRepeating("DealBurningDamage", 0, 1.0f);
        }
        else if (!burningUp)
        {
            
            CancelInvoke("DealBurningDamage"); // If burningUp becomes false, stop the repeating method
        }

        if (!tutorialCleared)
        {
            TutorialCheck();
        } else if(!spawnEnabled)
        {
            EnableSpawnPoint();
        }
        if (!GG)
        {
            GameOverCheck();
        }
        PlayerStatCheck();
        //Debug.Log("Speed" + playerSpeed + ", Altitude" + distanceToGoal);
    }

    void GameOverCheck()
    {
        playerMovement.enabled = !playerStats.isDead;
        HUDUI.SetActive(!playerStats.isDead);
        GameOverScreen.SetActive(playerStats.isDead);
        GG = playerStats.isDead;
        if (GG) 
        { 
            SetScore(); 
        }
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
    private void ClearTutorialArea()
    {
        tutorialZone.SetActive(false);
        controlsUI.SetActive(false);
        tutorialCleared = true;
        //Debug.Log("ClearingTutorialArea");
    }
    private void TutorialCheck()
    {
        if (distanceToGoal <= 19800 && !dialogueBox.activeSelf)
        {
            ClearTutorialArea();
        }
    }
    private void EnableSpawnPoint()
    {
        spawnPoints.SetActive(true);
        spawnEnabled = true;
    }
    private void CheckSpeed()
    {
        burningUp = (playerSpeed >= TooFastThreshold);
    }
    // This method will be invoked repeatedly to deal burning damage
    void DealBurningDamage()
    {
        playerStats.TakeDamage(BurnDamage);
    }
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void SetHUDColor()
    {
        if(playerSpeed > TooFastThreshold)
        {
            myTextObject.fontMaterial = burnUpMaterial2;
        }else if(playerSpeed > FastThreshold)
        {
            myTextObject.fontMaterial = FastMaterial1;
        }else if(playerSpeed > slowDownThreshold)
        {
            myTextObject.fontMaterial = slowDownMaterial1;
        }
        else
        {
            myTextObject.fontMaterial = normalMaterial1;
        }
    }
    void SetScore()
    {
        finalScore.text = "Score: " + distanceToGoal;
        SetTopScore();
    }
    void SetTopScore()
    {
        if(distanceToGoal < tst.GetTopScore())
        {
            tst.SetTopScore( (int)distanceToGoal);
        }
    }
}
