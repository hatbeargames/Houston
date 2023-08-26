using UnityEngine;
using TMPro;

public class SpeedCalculator : MonoBehaviour
{
    public GameObject player;
    [SerializeField] TMP_Text sped;
    [SerializeField] GameManager gm;
    Rigidbody2D rb;
    private void Start()
    {
        player = GameObject.Find("SpaceCraft");
        //gm = GameObject.Find("GameManger").GetComponent<GameManager>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Convert velocity from m/s to km/h (1 m/s = 3.6 km/h)
        float speedInKmPerHour = rb.velocity.magnitude * 3.6f;
        //Debug.Log("Speed in km/h: " + speedInKmPerHour);
        sped.text = (int)speedInKmPerHour + "km/h";
        gm.SetSpeed((int)speedInKmPerHour);
    }
}