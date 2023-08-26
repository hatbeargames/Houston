using UnityEngine;

public class TrackAndDestroy : MonoBehaviour
{
     // Name of the player object.
    public float maxDistance = 100f;      // Distance threshold.

    private GameObject player;

    private void Start()
    {
        // Find the player object by its name.
        player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {
            Debug.LogWarning("Player object not found. Please ensure it's named correctly.");
        }
    }

    private void Update()
    {
        if (player)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > maxDistance)
            {
                Destroy(gameObject); // Destroy the object this script is attached to.
            }
        }
    }
}
