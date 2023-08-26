using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Rigidbody2D rb;
    private int playerLayer;
    public int valMin;
    public int valMax;
    private int value;
    [SerializeField] private GameObject player;
    private PlayerStats playerStats;

    private void Start()
    {
        player = GameObject.Find("SpaceCraft");
        value = Random.Range(valMin, valMax);
        playerStats = player.GetComponent<PlayerStats>();

        // Initialize the Rigidbody2D for the item to interact with other physics objects
        //rb = GetComponent<Rigidbody2D>();
        //if (rb == null)
        //{
        //    rb = gameObject.AddComponent<Rigidbody2D>();
        //}
        //rb.gravityScale = 0;  // Assuming you want no gravity so it floats in space
        //rb.freezeRotation = true;  // Assuming you don't want it to rotate on collision

        // Fetch the layer by name
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object we collided with is on the Player layer
        if (collision.gameObject.layer == playerLayer)
        {
            ConsumeItem(collision.gameObject);
            Destroy(gameObject);  // Destroy the pickup after consumption
        }
    }

    private void ConsumeItem(GameObject player)
    {
        switch (gameObject.tag)
        {
            case "HealthPickup":
                // Handle health pickup
                Debug.Log("Health Item Consumed!"+value);
                playerStats.AddToHealth(value);
                break;

            case "EnergyPickup":
                // Handle ammo pickup
                Debug.Log("Energy Item Consumed!"+ value);
                playerStats.AddToEnergy(value);
                break;

            case "FuelPickup":
                // Handle power up
                Debug.Log("Fuel Item Consumed!"+ value);
                playerStats.AddToThrusters(value);
                break;

            default:
                Debug.Log("Unknown item type!");
                break;
        }

        
    }
}
