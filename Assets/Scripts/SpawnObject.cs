using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public void SpawnRandomObject(GameObject go)
    {
        GameObject instance = (GameObject)Instantiate(go, transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }
}
