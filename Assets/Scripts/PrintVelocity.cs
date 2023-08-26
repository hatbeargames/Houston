using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintVelocity : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerMovement pm;
    // Start is called before the first frame update
    void Start()
    {
        rb= GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        InvokeRepeating("PrintVel", 0f, 1f);
    }
    void PrintVel()
    {
        if (rb)
        {
            Debug.Log("Clamped Vel:" + pm.verticalVelocity);
            Debug.Log("Actual Vel:"+rb.velocity.y);
        }
    }
}
