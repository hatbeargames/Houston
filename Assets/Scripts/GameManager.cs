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
    [SerializeField] private GameObject playerSFX;
    [SerializeField] private GameObject winningPlatform;
    [SerializeField] private GameObject gameWonUI;
    public TMP_Text myTextObject;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private float playerSpeed;
    private float TooFastThreshold = 140;
    private float slowDownThreshold;
    private float FastThreshold;
    private float startingDistance;
    private float distanceToGoal = 5000;
    private bool tutorialCleared = false;
    private bool spawnEnabled = false;
    private bool nearingGoal = false;
    public int BurnDamage;
    private bool burningUp;
    private TopScoreTracker topScoreTracker;

    //End game checks
    private bool endGame_PlatformSet = false;
    private bool endGame_SpawnPoints;
    private bool endGame_WinningScreen;
    private bool endGame_PlayerMovement;
    void Start()
    {
        startingDistance = distanceToGoal;
        topScoreTracker = FindAnyObjectByType<TopScoreTracker>();
        slowDownThreshold = TooFastThreshold * .50f;
        FastThreshold = TooFastThreshold * .75f;
        playerMovement = player.GetComponent<PlayerMovement>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        GameWonCheck();
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
        } else if(!spawnEnabled && !nearingGoal)
        {
            EnableSpawnPoint();
        }
        if (!GG)
        {
            GameOverCheck();
        }
        PlayerStatCheck();

        if (nearingGoal)
        {
            SetUpEndGame();
        }
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
            playerSFX.SetActive(!GG);
        }
    }
    void GameWonCheck()
    {
        nearingGoal = (distanceToGoal < startingDistance * 0.01f);
        if(distanceToGoal < startingDistance * 0.01f)
        {
            //Debug.Log("You won the game!");
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
        if (distanceToGoal <= startingDistance * 0.985f && !dialogueBox.activeSelf)
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
        burningUp = (playerSpeed >= TooFastThreshold) && player.isSh;
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
        if(distanceToGoal < topScoreTracker.GetTopScore())
        {
            topScoreTracker.SetTopScore( (int)distanceToGoal);
        }
    }

    void SetUpEndGame()
    {
        if (!endGame_PlatformSet)
        {
            //Debug.Log("Setting Up Platform: " + (player.transform.position.y - distanceToGoal + ", Players Pos: "+player.transform.position.y));
            GameObject instantiatedPlatform = Instantiate(winningPlatform, new Vector3(player.transform.position.x, player.transform.position.y - distanceToGoal, 0), Quaternion.identity);
            endGame_PlatformSet = true;
        }
        if (!endGame_PlayerMovement)
        {
            playerMovement.enabled = false;
            endGame_PlayerMovement = true;
        }
        if (!endGame_SpawnPoints)
        {
            spawnPoints.SetActive(false);
            endGame_SpawnPoints = true;
        }
        if (!endGame_WinningScreen)
        {
            gameWonUI.SetActive(true);
            endGame_WinningScreen = true;
        }

    }
}
