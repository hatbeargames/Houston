using UnityEngine;

public class ChanceToSpawn : MonoBehaviour
{
    [Tooltip("Objects to randomly spawn")]
    public GameObject[] prefabs;
    public float spawnChance;


    public void SpawnRandomObject(float percent)
    {
        spawnChance = percent;
        // On start, decide whether to spawn based on the percentage chance
        if (Random.Range(0f, 100f) <= spawnChance && prefabs.Length > 0)
        {
            GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];

            // Instantiate it at the position of the attached game object
            Instantiate(selectedPrefab, transform.position, Quaternion.identity);
        }
    }
}
