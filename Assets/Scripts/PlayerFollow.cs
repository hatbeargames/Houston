using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = Vector3.zero;
        player = GameObject.Find("SpaceCraft").transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
