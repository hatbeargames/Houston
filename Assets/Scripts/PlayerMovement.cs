using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    
    //GAMEOBJECTS
    [SerializeField] GameObject L_Thruster;
    [SerializeField] GameObject R_Thruster;
    [SerializeField] GameObject L_Dash_Thruster;
    [SerializeField] GameObject R_Dash_Thruster;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject ShieldColliderObject;
    public PlayerStats playerStats;
    private CircleCollider2D shieldCollider;

    //PHYSICS VARIABLES
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed
    private Quaternion targetRotation;
    public float maxRotationAngle = 45f;
    public float defaultRotationSpeed;
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
    private bool isFloating = false;  // Starts as false since player is not floating initially

    Vector2 moveDirection = Vector2.zero;
    public InputAction playerControls;
    LaserGun lg;
    public float verticalVelocity;
    public float dashSpeed = 100f;
    public float dashDuration = 3f;
    public float maxTimeBetweenTaps = 0.3f; // Time allowed between double taps
    private float lastTapTimeLeft = 0f;
    private float lastTapTimeRight = 0f;
    private bool isDashing = false;


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
        defaultRotationSpeed = rotationSpeed;
        shieldCollider = ShieldColliderObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleFloating();
        }

        if (playerStats.currentThrusters == playerStats.GetMaxThrust())
        {
            thrustersRecharged = true;
        }

        if(playerStats.currentEnergy == playerStats.GetMaxEnergy())
        {
            energyRecharged = true;
        }
        CheckThrusterStats();
        if (isDashing && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            isDashing = false;
        }
        moveDirection = playerControls.ReadValue<Vector2>();
        if (playerStats.currentThrusters > 0 && thrustersRecharged)
        {
            isLeftThrusterActive = Input.GetKey(KeyCode.D);
            isRightThrusterActive = Input.GetKey(KeyCode.A);
            isUpThrusterActive = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        }
        else
        {
            thrustersRecharged = false;
            isLeftThrusterActive = false;
            isRightThrusterActive = false;
            isUpThrusterActive = false;
        }
        L_Thruster.SetActive(isUpThrusterActive || isLeftThrusterActive || isFloating);
        R_Thruster.SetActive(isUpThrusterActive || isRightThrusterActive || isFloating);
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
        shieldCollider.enabled = isShieldActive;
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
        //if (isLeftThrusterActive || isRightThrusterActive || isUpThrusterActive)
        if (isUpThrusterActive || isDashing )
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
            //Original Gravity multiplie
            //gravityMultiplier += gravityIncreaseRate * Time.deltaTime;

            //Sam as above but includes the floating check
            if (!isFloating)
            {
                gravityMultiplier += gravityIncreaseRate * Time.deltaTime;
                
            }
            else
            {
                gravityMultiplier = 0f;  // No gravity applied when floating
                playerStats.ConsumeThrusters(thrusterConsumptionRate * Time.deltaTime);
            }
            gravityMultiplier = Mathf.Clamp(gravityMultiplier, -1f, maxGravityMultiplier);
        }

        // Double tap detection for left (A key)
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastTapTimeLeft < maxTimeBetweenTaps)
            {
                rotationSpeed = rotationSpeed * 2;
                StartCoroutine(Dash(-Vector2.right,L_Dash_Thruster));  // Negative for left direction
            }
            else
            {
                rotationSpeed = defaultRotationSpeed;
            }
            lastTapTimeLeft = Time.time;
        }

        // Double tap detection for right (D key)
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastTapTimeRight < maxTimeBetweenTaps)
            {
                rotationSpeed = rotationSpeed * 2;
                StartCoroutine(Dash(Vector2.right,R_Dash_Thruster));   // Positive for right direction
            }
            else
            {
                rotationSpeed = defaultRotationSpeed;
            }
            lastTapTimeRight = Time.time;
        }
    }
    private void FixedUpdate()
    {

        //Calculate and clamp the vertical velocity so it doesn't compound.
        verticalVelocity = moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier;
        verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);
        #region Original Velocity and rotation code
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
        #endregion

        #region v2 velocity and rotation code
        ////Needed at the beginning of the various velocity components.
        //if (isDashing)
        //{
        //    // If the player is dashing, we don't need to apply other forces or checks.
        //    // This will be set by the Dash coroutine
        //} else if ((isUpThrusterActive || isRightThrusterActive || isLeftThrusterActive) && thrustersRecharged)
        //{
        //    //Debug.Log("Applying Velocity");
        //    rb.velocity = new Vector2(moveDirection.x * moveSpeed, verticalVelocity);
        //    //rb.velocity = new Vector2(moveDirection.x * moveSpeed, Mathf.Clamp((moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier), minVerticalSpeed, maxVerticalSpeed));
        //}
        //else if (!playerStats.isDead)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, verticalVelocity);
        //}
        ////float currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.z, 360f);
        //float currentRotation;
        //// Calculate target rotation based on thrusters
        //if (isUpThrusterActive && !isRightThrusterActive && !isLeftThrusterActive)
        //{
        //    targetRotation = Quaternion.Euler(0f, 0f, defaultRotationAngle);
        //}
        //else if (isLeftThrusterActive && !isRightThrusterActive)
        //{
        //    currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.z, -360f);
        //    float targetRotationFloat = currentRotation - rotationSpeed;
        //    targetRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(targetRotationFloat, -maxRotationAngle, maxRotationAngle));
        //}
        //else if (isRightThrusterActive && !isLeftThrusterActive)
        //{
        //    currentRotation = Mathf.Repeat(transform.rotation.eulerAngles.z, 360f);
        //    float targetRotationFloat = currentRotation + rotationSpeed;
        //    targetRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(targetRotationFloat, (-1 * maxRotationAngle), maxRotationAngle));
        //}
        //else
        //{
        //    targetRotation = Quaternion.Euler(0f, 0f, defaultRotationAngle);
        //}

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        #endregion

        #region v3 velocity and rotation code
        //Calculate and clamp the vertical velocity so it doesn't compound.
        verticalVelocity = moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier;
        verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);

        // If player is not dashing, apply standard horizontal velocity.
        if (!isDashing)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        }
        // Otherwise, dashing velocity is set in the coroutine.

        // If the up thruster is active or the player is not dead, adjust vertical velocity.
        if (isUpThrusterActive || !playerStats.isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalVelocity);
        }

        // Handling rotation based on thrusters
        float currentRotation;
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
        #endregion
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

    IEnumerator Dash(Vector2 direction, GameObject thruster)
    {
        isDashing = true;
        thruster.SetActive(true);
        float startTime = Time.time;
        //Original Velocity adjustments, time based
        //while (Time.time - startTime < dashDuration)
        //{
        //    rb.velocity = new Vector2(direction.x * dashSpeed, rb.velocity.y);
        //    yield return null;
        //}

        //Adding force to velocity, time based
        //rb.AddForce(direction * dashSpeed, ForceMode2D.Impulse);  // Apply impulse force
        //yield return new WaitForSeconds(dashDuration);

        //Velocity that keeps going while the key is pressed down.
        while (isDashing && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            rb.velocity = new Vector2(direction.x * dashSpeed, rb.velocity.y);
            yield return null; // Wait until the next frame
        }
        thruster.SetActive(false);
        isDashing = false;
    }

    void ToggleFloating()
    {
        if (isFloating)
        {
            // Disable floating and reset gravity
            gravityMultiplier = 1.0f;
        }
        else
        {
            // Enable floating
            gravityMultiplier = 0.0f;  // 0 gravity multiplier will make them float in place
        }

        isFloating = !isFloating;  // Toggle the floating status
    }
}
