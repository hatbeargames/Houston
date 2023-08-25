using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    public Transform target; // Drag the target object here in the inspector
    public float rotationSpeed = 5f; // Speed at which the object rotates towards the target
    
    private void Start()
    {
        target = GameObject.Find("SpaceCraft").transform; 
    }
    void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        // Find the vector pointing from the object to the target
        Vector2 direction = target.position - transform.position;

        // Calculate the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate towards that angle
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); // Subtract 90 to account for the default sprite orientation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
