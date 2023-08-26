using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    
    //GAMEOBJECTS
    [SerializeField] GameObject L_Thruster;
    [SerializeField] GameObject R_Thruster;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject ShieldColliderObject;
    public PlayerStats playerStats;

    //PHYSICS VARIABLES
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed
    private Quaternion targetRotation;
    public float maxRotationAngle = 45f;
    public float defaultRotationAngle = 0f;
    public float gravityMultiplier = 1.0f;
    public float gravityIncreaseRate = 0.1f; // Adjust how quickly you want gravity to increase
    public float maxGravityMultiplier = 1f;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float minVerticalSpeed = -10f;  // Adjust as needed for max fall speed
    public float maxVerticalSpeed = 5f;    // Adjust as needed for max upward speed
    public float thrusterConsumptionRate = 10f;
    public float energyConsumptionRate = 10f;

    //INPUT VARIABLES
    bool isLeftThrusterActive;
    bool isRightThrusterActive;
    bool isUpThrusterActive;
    bool isShieldActive;
    bool wasEnergyUsed = false;
    private bool wereThrustersActive = false;
    bool thrustersRecharged = true;
    bool energyRecharged = true;
    Vector2 moveDirection = Vector2.zero;
    public InputAction playerControls;
    LaserGun lg;
    public float verticalVelocity;
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        lg = FindObjectOfType<LaserGun>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStats.currentThrusters == playerStats.GetMaxThrust())
        {
            thrustersRecharged = true;
        }

        if(playerStats.currentEnergy == playerStats.GetMaxEnergy())
        {
            energyRecharged = true;
        }
        CheckThrusterStats();
        moveDirection = playerControls.ReadValue<Vector2>();
        if (playerStats.currentThrusters > 0 && thrustersRecharged)
        {
            isLeftThrusterActive = Input.GetKey(KeyCode.D);
            isRightThrusterActive = Input.GetKey(KeyCode.A);
            isUpThrusterActive = Input.GetKey(KeyCode.W);
        }
        else
        {
            thrustersRecharged = false;
            isLeftThrusterActive = false;
            isRightThrusterActive = false;
            isUpThrusterActive = false;
        }
        L_Thruster.SetActive(isUpThrusterActive || isLeftThrusterActive);
        R_Thruster.SetActive(isUpThrusterActive || isRightThrusterActive);
        if (playerStats.currentEnergy > 0)
        {
            isShieldActive = Input.GetMouseButton(1);
        }
        else
        {
            energyRecharged = false;
            isShieldActive = false;
        }
        Shield.SetActive(isShieldActive);
        ShieldColliderObject.GetComponent<CircleCollider2D>().enabled = isShieldActive;
        CheckEnergyStats();
        if (isShieldActive || lg.isFiring)
        {
            if (!wasEnergyUsed)
            {
                wasEnergyUsed = true;
                playerStats.StopEnergyBarLerpBack();
            }
            if (isShieldActive && lg.isFiring)
            {
                playerStats.ConsumeEnergy((energyConsumptionRate + energyConsumptionRate) * Time.deltaTime);
            }
            else
            {
                playerStats.ConsumeEnergy(energyConsumptionRate * Time.deltaTime);
            }
        }
        else
        {
            if (wasEnergyUsed)
            {
                wasEnergyUsed = false;
            }
        }
        if (isLeftThrusterActive || isRightThrusterActive || isUpThrusterActive)
        {
            if (!wereThrustersActive) // This means the thrusters were just activated
            {
                wereThrustersActive = true;
                playerStats.StopThrusterBarLerpBack();
            }
                gravityMultiplier = 1.0f;
                playerStats.ConsumeThrusters(thrusterConsumptionRate * Time.deltaTime);
        }
        else
        {
            if (wereThrustersActive) // This means the thrusters were just deactivated
            {
                wereThrustersActive = false;
            }
            gravityMultiplier += gravityIncreaseRate * Time.deltaTime;
            gravityMultiplier = Mathf.Clamp(gravityMultiplier, -1f, maxGravityMultiplier);
        }
    }
    private void FixedUpdate()
    {

        //Calculate and clamp the vertical velocity so it doesn't compound.
        verticalVelocity = moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier;
        verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);

        //float verticalVelocity = rb.velocity.y - gravityMultiplier;
        //verticalVelocity = rb.velocity.y - (moveDirection.y * moveSpeed * gravityMultiplier);
        //if(rb.velocity.y > 0) 
        //{
        //    verticalVelocity = moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier;
        //    verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);
        //}
        //else if(rb.velocity.y < 0) 
        //{
        //    //should not apply movespeed or moveDirection to the falling velocity.
        //    verticalVelocity = rb.velocity.y * gravityMultiplier;
        //    verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);
        //}
        //else
        //{
        //    verticalVelocity = moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier;
        //    verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);
        //}

        if ((isUpThrusterActive || isRightThrusterActive || isLeftThrusterActive) && thrustersRecharged)
        {
            //Debug.Log("Applying Velocity");
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, verticalVelocity);
            //rb.velocity = new Vector2(moveDirection.x * moveSpeed, Mathf.Clamp((moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier), minVerticalSpeed, maxVerticalSpeed));
        }
        else if (!playerStats.isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalVelocity);
        }
        //float currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.z, 360f);
        float currentRotation;
        // Calculate target rotation based on thrusters
        if (isUpThrusterActive && !isRightThrusterActive && !isLeftThrusterActive)
        {
            targetRotation = Quaternion.Euler(0f, 0f, defaultRotationAngle);
        }
        else if (isLeftThrusterActive && !isRightThrusterActive)
        {
            currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.z, -360f);
            float targetRotationFloat = currentRotation - rotationSpeed;
            targetRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(targetRotationFloat, -maxRotationAngle, maxRotationAngle));
        }
        else if (isRightThrusterActive && !isLeftThrusterActive)
        {
            currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.z, 360f);
            float targetRotationFloat = currentRotation + rotationSpeed;
            targetRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(targetRotationFloat, (-1 * maxRotationAngle), maxRotationAngle));
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, defaultRotationAngle);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void CheckThrusterStats()
    {
        if ((playerStats.currentThrusters >= (playerStats.GetMaxThrust() * playerStats.GetAuxillaryThrusterThreshold()))&& !thrustersRecharged)
        {
            //Debug.Log("In range to set thrusters to max");
            playerStats.StopThrusterBarLerpBack();
            playerStats.currentThrusters = playerStats.GetMaxThrust();
            energyRecharged = true;
        }
    }
    void CheckEnergyStats()
    {
        if ((playerStats.currentEnergy >= (playerStats.GetMaxEnergy() * playerStats.GetAuxillaryEnergyThreshold())) && !energyRecharged)
        {
            Debug.Log("In range to set energy to max");
            playerStats.StopEnergyBarLerpBack();
            playerStats.currentEnergy = playerStats.GetMaxEnergy();
            energyRecharged = true;
        }
    }
    public bool GetEnergyStatus()
    {
        return energyRecharged;
    }
    public bool GetShieldStatus()
    {
        return isShieldActive;
    }

    public bool GetThrusterStatus()
    {
        return thrustersRecharged;
    }
}
