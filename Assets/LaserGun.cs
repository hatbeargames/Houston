using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public float offsetAngle;
    [SerializeField] LineRenderer lr;
    public bool isFiring;
    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject maxLaserDistance;
    [SerializeField] Material aimingLaser;
    [SerializeField] Material firingLaser;
    [SerializeField] Color aimingColor;
    [SerializeField] Color firingColor;
    [SerializeField] PlayerMovement pm;
    float aimingWidth = 0.007f;
    public float firingWidth = .05f;
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
        isFiring = Input.GetMouseButton(0);
        DrawLaser();
        if (isFiring)
        {
            Debug.Log("Laser Fired");
            //lr.material = firingLaser;
            //lr.SetWidth(firingWidth, firingWidth);
            lr.startWidth = firingWidth;
            lr.endWidth = firingWidth;
            lr.startColor = firingColor;
            lr.endColor = firingColor;
        }
        else
        {
            //lr.material = aimingLaser;
            lr.startWidth = aimingWidth;
            lr.endWidth = aimingWidth;
            lr.startColor = aimingColor;
            lr.endColor = aimingColor;
        }
    }
    void DrawLaser()
    {
        lr.SetPosition(0, firePoint.transform.position);
        lr.SetPosition(1, maxLaserDistance.transform.position);
    }
    float AngleDelta(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


}
