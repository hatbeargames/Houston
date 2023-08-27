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
    [SerializeField] GameObject laserParticles;
    [SerializeField] Material aimingLaser;
    [SerializeField] Material firingLaser;
    [SerializeField] Color aimingColor;
    [SerializeField] Color firingColor;
    [SerializeField] PlayerMovement pm;
    [SerializeField] PlayerStats ps;
    [SerializeField] EdgeCollider2D laserCollider;
    float aimingWidth = 0.007f;
    public float firingWidth = .05f;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        ps = GameObject.Find("SpaceCraft").GetComponent<PlayerStats>();
        Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleDelta(posOnScreen, mouseOnScreen) + offsetAngle;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        laserCollider.points = new Vector2[2] { firePoint.transform.position, firePoint.transform.position };
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFiring)
        {
            laserCollider.points = new Vector2[2] { firePoint.transform.position, firePoint.transform.position };
        }
        if (!ps.isDead)
        {
            //Current coordinates of the laser Gun
            Vector2 posOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Finds the Vector3 of the mouse position and cast it into a Vector2.
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

            //Function to find the delta of the angle between the two position.
            float angle = AngleDelta(posOnScreen, mouseOnScreen) + offsetAngle;

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            if (ps.currentEnergy > 0)
            {
                //Debug.Log("Laser Firing!");
                isFiring = Input.GetMouseButton(0);
            }
            else
            {
                isFiring = false;
            }
            laserParticles.SetActive(isFiring);
            DrawLaser();
            if (isFiring)
            {
                lr.startWidth = firingWidth;
                lr.endWidth = firingWidth;
                lr.startColor = firingColor;
                lr.endColor = firingColor;
            }
            else
            {
                lr.startWidth = aimingWidth;
                lr.endWidth = aimingWidth;
                lr.startColor = aimingColor;
                lr.endColor = aimingColor;
            }
        }
    }
    void DrawLaser()
    {
        lr.SetPosition(0, firePoint.transform.position);
        lr.SetPosition(1, maxLaserDistance.transform.position);
        if (isFiring)
        {
            UpdateLaserCollider();
        }
    }
    float AngleDelta(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void UpdateLaserCollider()
    {
        //Inverse not needed when edge collider is not child of objects used.
        //Vector2 localStartPos = transform.InverseTransformPoint(firePoint.transform.position);
        //Vector2 localEndPos = transform.InverseTransformPoint(maxLaserDistance.transform.position);
        //Debug.DrawLine(localStartPos, localEndPos, Color.red);
        //Debug.DrawLine(firePoint.transform.position, maxLaserDistance.transform.position, Color.blue);
        laserCollider.points = new Vector2[2] { firePoint.transform.position, maxLaserDistance.transform.position };
    }
}
