using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerMovement : MonoBehaviour
{
    
    
    //GAMEOBJECTS
    [SerializeField] GameObject L_Thruster;
    [SerializeField] GameObject R_Thruster;
    [SerializeField] GameObject Shield;
    public PlayerStats playerStats;
    [SerializeField] public CinemachineBrain camBrain;
    Coroutine camUpdateCoroutine;
    //PHYSICS VARIABLES
    public float rotationSpeed = 5f; // Adjust the rotation speed as needed
    private Quaternion targetRotation;
    public float maxRotationAngle = 22.5f;
    public float defaultRotationAngle = 0f;
    private float gravityMultiplier = 1.0f;
    public float gravityIncreaseRate = 0.1f; // Adjust how quickly you want gravity to increase
    public float maxGravityMultiplier = 5.0f;
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
    bool isRotating = false;
    Coroutine rotationCoroutine;

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
        camBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();
        if (playerStats.currentThrusters == playerStats.GetMaxThrust())
        {
            thrustersRecharged = true;
        }

        if(playerStats.currentEnergy == playerStats.GetMaxEnergy())
        {
            energyRecharged = true;
        }
        CheckThrusterStats();
        bool bothLeftAndRight = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D);
        
        if (playerStats.currentThrusters > 0 && thrustersRecharged)
        {
            //Original Logic
            isLeftThrusterActive = (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D));
            isRightThrusterActive = (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A));
            isUpThrusterActive = Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A));
            //isRightThrusterActive = !bothLeftAndRight && (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W)));
            //isLeftThrusterActive = !bothLeftAndRight && (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)));
            //isUpThrusterActive = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D);
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
                playerStats.ConsumeThrusters(thrusterConsumptionRate * Time.deltaTime);
        }
        else
        {
            if (wereThrustersActive) // This means the thrusters were just deactivated
            {
                wereThrustersActive = false;
            }

        }
        isRotating = isUpThrusterActive || isRightThrusterActive || isLeftThrusterActive;
        //Debug.Log(isRotating);
        // Handle the rotation coroutine

        //ManuallyUpdateCamera(Time.deltaTime);
        //if ((isLeftThrusterActive || isRightThrusterActive || isUpThrusterActive || isShieldActive || lg.isFiring) && camUpdateCoroutine == null)
        //{
        //    camUpdateCoroutine = StartCoroutine(ManuallyUpdateCameraCoroutine());
        //}
        //Debug.Log("UpThrusters: " + isUpThrusterActive + ", LeftThrusterActive: " + isLeftThrusterActive + ", RightThruster: " + isRightThrusterActive);
    }
    private void FixedUpdate()
    {

        if (isRotating && rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(RotationCoroutine());
        }
        else if (!isRotating && rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
        //Calculate and clamp the vertical velocity so it doesn't compound.
        float verticalVelocity = moveDirection.y * moveSpeed + rb.velocity.y * gravityMultiplier;
        verticalVelocity = Mathf.Clamp(verticalVelocity, minVerticalSpeed, maxVerticalSpeed);
        if ((isUpThrusterActive || isRightThrusterActive || isLeftThrusterActive) && thrustersRecharged)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, verticalVelocity);
            gravityMultiplier = 1.0f;
        }
        else
        {
            gravityMultiplier += gravityIncreaseRate * Time.deltaTime;
            gravityMultiplier = Mathf.Clamp(gravityMultiplier, 1.0f, maxGravityMultiplier);
        }

        //float currentRotationZ = NormalizeAngle(transform.rotation.eulerAngles.z);
        //float targetRotationZ = defaultRotationAngle; // Default value

        //if (isUpThrusterActive && !isRightThrusterActive && !isLeftThrusterActive)
        //{
        //    targetRotationZ = defaultRotationAngle;

        //}
        //else if (isLeftThrusterActive && !isRightThrusterActive)
        //{
        //    float desiredRotation = currentRotationZ - rotationSpeed;
        //    targetRotationZ = Mathf.Clamp(desiredRotation, -maxRotationAngle, maxRotationAngle);

        //}
        //else if (isRightThrusterActive && !isLeftThrusterActive)
        //{
        //    float desiredRotation = currentRotationZ + rotationSpeed;
        //    targetRotationZ = Mathf.Clamp(desiredRotation, -maxRotationAngle, maxRotationAngle);
        //}

        //float newRotationZ = Mathf.LerpAngle(currentRotationZ, targetRotationZ, rotationSpeed * Time.fixedDeltaTime);
        //transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
    }
    private void LateUpdate()
    {
        //camBrain.ManualUpdate();
    }
    void PerformRotation()
    {
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
        }
    }
    void CheckEnergyStats()
    {
        if ((playerStats.currentEnergy >= (playerStats.GetMaxEnergy() * playerStats.GetAuxillaryEnergyThreshold())) && !energyRecharged)
        {
            //Debug.Log("In range to set energy to max");
            playerStats.StopEnergyBarLerpBack();
            playerStats.currentEnergy = playerStats.GetMaxEnergy();
        }
    }
    public bool GetEnergyStatus()
    {
        return energyRecharged;
    }
    public bool GetThrusterStatus()
    {
        return thrustersRecharged;
    }
    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
    public void ManuallyUpdateCamera()
    {
        camBrain.ManualUpdate();
    }
    IEnumerator ManuallyUpdateCameraCoroutine()
    {
        while (isLeftThrusterActive || isRightThrusterActive || isUpThrusterActive || isShieldActive || lg.isFiring)
        {
            camBrain.ManualUpdate();
            yield return null;
        }
        camUpdateCoroutine = null;
    }

    IEnumerator RotationCoroutine()
    {
        while (isRotating)
        {
            float currentRotationZ = NormalizeAngle(transform.rotation.eulerAngles.z);
            float targetRotationZ = defaultRotationAngle;

            // ... [The rest of the rotation logic remains the same]
            if (isLeftThrusterActive && !isRightThrusterActive)
            {
                targetRotationZ = Mathf.Clamp(currentRotationZ - rotationSpeed, -maxRotationAngle, maxRotationAngle);
            }
            else if (isRightThrusterActive && !isLeftThrusterActive)
            {
                targetRotationZ = Mathf.Clamp(currentRotationZ + rotationSpeed, -maxRotationAngle, maxRotationAngle);
            }
            float newRotationZ = Mathf.LerpAngle(currentRotationZ, targetRotationZ, rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
            //camBrain.ManualUpdate();
            yield return null;
        }
        
    }
}
