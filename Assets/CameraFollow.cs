using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform.
    public float lerpSpeed = 5.0f; // The speed of the camera's movement.
    public Vector3 offset; // An optional offset to position the camera relative to the player.
    [SerializeField]public CinemachineCore camCore;
    [SerializeField] public CinemachineBrain camBrain;

    private void Start()
    {
        target = GameObject.Find("SpaceCraft").transform;
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }
    public void ManuallyUpdateCamera(float deltaTime)
    {
        camBrain.ManualUpdate();
    }
}
