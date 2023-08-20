using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public float offsetAngle;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleDelta(posOnScreen, mouseOnScreen) + offsetAngle;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    // Update is called once per frame
    void Update()
    {
        //Current coordinates of the laser Gun
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Finds the Vector3 of the mouse position and cast it into a Vector2.
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Function to find the delta of the angle between the two position.
        float angle = AngleDelta(posOnScreen, mouseOnScreen)+offsetAngle;

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    float AngleDelta(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


}
