using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public InputAction playerControls;
    [SerializeField] GameObject L_Thruster;
    [SerializeField] GameObject R_Thruster;
    [SerializeField] GameObject Shield;
    Vector2 moveDirection = Vector2.zero;
    public float rotationSpeed = 10f; // Adjust the rotation speed as needed
    private Quaternion targetRotation;
    public float maxRotationAngle = 45f;
    public float defaultRotationAngle = 0f;
    bool isLeftThrusterActive;
    bool isRightThrusterActive;
    bool isUpThrusterActive;
    bool isShieldActive;
    bool isFiring;

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

    }

    // Update is called once per frame
    void Update()
    {
        isShieldActive = Input.GetMouseButton(1);
        moveDirection = playerControls.ReadValue<Vector2>();

        isLeftThrusterActive = Input.GetKey(KeyCode.D);
        isRightThrusterActive = Input.GetKey(KeyCode.A);
        isUpThrusterActive = Input.GetKey(KeyCode.W);

        L_Thruster.SetActive(isUpThrusterActive || isLeftThrusterActive);
        R_Thruster.SetActive(isUpThrusterActive || isRightThrusterActive);
        Shield.SetActive(isShieldActive);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

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
            Debug.Log(targetRotationFloat);
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
}
