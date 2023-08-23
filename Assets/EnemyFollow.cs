using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;  // Drag your player transform here in the inspector
    public float speed = 5.0f;  // Speed at which the enemy will move towards the player

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("SpaceCraft").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("SpaceCraft").transform;
        }
        // Calculate the direction towards the player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();  // Convert to unit vector

        // Move the enemy towards the player
        transform.position += direction * speed * Time.deltaTime;
    }
}
