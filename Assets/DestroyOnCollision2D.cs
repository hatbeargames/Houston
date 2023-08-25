using UnityEngine;

public class DestroyOnCollision2D : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the game object this script is attached to
        Debug.Log("Collided");
        Destroy(gameObject);
    }
    
}
