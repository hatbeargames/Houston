using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int damage;  // Set in insepctor . Damage this enemy deals on collision
    public Coroutine DamageCoroutine { get; set; }

    private PlayerCollision playerCollision;  // reference to the PlayerCollision script

    private void Start()
    {
        // Assuming the player has a tag "Player", adjust if your player has a different tag
        playerCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollision>();
    }
    private void OnDestroy()
    {
        if (DamageCoroutine != null)
        {
            if (playerCollision != null)
            {
                playerCollision.StopSpecificCoroutine(DamageCoroutine);
                DamageCoroutine = null;
            }
        }
    }

}
