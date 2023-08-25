using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public SpawnObject[] spawnPoints;
    public GameObject[] objects;
    public GameObject objectToSpawn;
    [Tooltip("Rate at which objects spawn, in seconds.")]
    [SerializeField] float spawnRate; //In seconds

    private void Start()
    {
        GameObject[] foundSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        // Initialize the spawnPoints array with the same size as foundSpawnPoints
        spawnPoints = new SpawnObject[foundSpawnPoints.Length];

        // Populate the spawnPoints array with the SpawnObject component from each found spawn point
        for (int i = 0; i < foundSpawnPoints.Length; i++)
        {
            spawnPoints[i] = foundSpawnPoints[i].GetComponent<SpawnObject>();
        }
        // Start calling the spawn function at regular intervals
        InvokeRepeating(nameof(SpawnFromRandomPoint), 0f, spawnRate);
    }

    void SpawnFromRandomPoint()
    {
        // Choose a random spawn point and call its SpawnEnemy function
        int randomIndex = Random.Range(0, spawnPoints.Length);
        int randomObject = Random.Range(0, objects.Length);
        objectToSpawn = objects[randomObject];
        spawnPoints[randomIndex].SpawnRandomObject(objectToSpawn);
    }
}
