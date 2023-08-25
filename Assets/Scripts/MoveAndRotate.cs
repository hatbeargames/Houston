using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveAndRotate : MonoBehaviour
{
    [Header("Movement Parameters")]
    [Tooltip("Array of game objects representing potential ending points of the path")]
    [SerializeField] private GameObject[] endObjects;

    [Tooltip("Minimum speed at which the object moves across the path")]
    [SerializeField] private float minMoveSpeed = 0.5f;

    [Tooltip("Maximum speed at which the object moves across the path")]
    [SerializeField] private float maxMoveSpeed = 2.0f;

    [SerializeField]private float moveSpeed;

    [Header("Rotation Parameters")]
    [Tooltip("Minimum rotation speed in degrees per second")]
    [SerializeField] private float minRotationSpeed = 5.0f;

    [Tooltip("Maximum rotation speed in degrees per second")]
    [SerializeField] private float maxRotationSpeed = 20.0f;

    [SerializeField] private float rotationSpeed;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Rigidbody2D rb;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        GameObject[] foundSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        // Initialize the spawnPoints array with the same size as foundSpawnPoints
       endObjects = new GameObject[foundSpawnPoints.Length];

        // Populate the spawnPoints array with the SpawnObject component from each found spawn point
        for (int i = 0; i < foundSpawnPoints.Length; i++)
        {
            endObjects[i] = foundSpawnPoints[i];
        }
        // Set the startPoint to the object's current position
        startPoint = transform.position;

        // Randomly pick one of the end objects and extract its position
        if (endObjects.Length > 0)
        {
            GameObject chosenEndObject = endObjects[Random.Range(0, endObjects.Length)];
            endPoint = chosenEndObject.transform.position;
        }

        // Randomly set moveSpeed and rotationSpeed within their respective ranges
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        // Initialize the Rigidbody2D reference
        rb = GetComponent<Rigidbody2D>();

        // Calculate the journey length
        journeyLength = Vector2.Distance(startPoint, endPoint);

        // Record the start time of the journey
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        // Calculate how far along the journey we are as a proportion of the total journey.
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = 0f;
        if (journeyLength != 0f)
        {
            fractionOfJourney = distCovered / journeyLength;
        }
        // Set our position as a proportion of the distance between the markers.
        rb.position = Vector2.Lerp(startPoint, endPoint, fractionOfJourney);

        // Rotate the object
        rb.rotation += rotationSpeed * Time.fixedDeltaTime;
    }
}

