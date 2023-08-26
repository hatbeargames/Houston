using UnityEngine;

public class RandomScale : MonoBehaviour
{
    // Define the range of possible scales
    public float minScale;
    public float maxScale;

    void Awake()
    {
        // Randomly determine a scale value between the min and max values
        float randomScale = Random.Range(minScale, maxScale);

        // Apply the uniform scale to the object
        transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}
