using UnityEngine;
using TMPro;

public class DistanceCalculator : MonoBehaviour
{
    public GameObject player;
    public Vector3 target;
    [SerializeField] TMP_Text alt;
    public int goalDistance = 20000;
    [SerializeField] GameManager gm;
    PlayerStats ps;
    
    private void Start()
    {
        target = new Vector3(0, player.transform.position.y - goalDistance);
        //gm = GameObject.Find("GameManger").GetComponent<GameManager>();
        ps = player.GetComponent<PlayerStats>();
    }
    void Update()
    {
        // Calculate altitude
        float altitudeInUnityUnits = CalculateAltitude();
        float altitudeInKm = Mathf.Abs(altitudeInUnityUnits);
        //Debug.Log("Altitude in Kilometers: " + altitudeInKm + "Target Position" + target.y + "Player" + player.transform.position.y);
        ps.SetAltitude(altitudeInKm);
        alt.text = "Altitude:" + (int)altitudeInKm + "Km";
        gm.SetDistanceToGoal((int)altitudeInKm);
    }

    float CalculateAltitude()
    {
        // Calculate the target position based on the player's position, 20,000 units below
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y - target.y, player.transform.position.z);
        return Mathf.Abs(player.transform.position.y) - Mathf.Abs(targetPosition.y);
    }
}
