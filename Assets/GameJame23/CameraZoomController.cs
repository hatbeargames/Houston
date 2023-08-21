using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera virtualCamera;
    public Rigidbody2D playerRigidbody;

    [Header("Zoom Parameters")]
    public float baseOrthoSize;
    public float maxOrthoSize;
    public float speedMultiplier;
    public float zoomSpeed;
    // Start is called before the first frame update
    private void Update()
    {
        AdjustCameraZoom();
    }

    // Update is called once per frame
    void AdjustCameraZoom()
    {
        float tgt_OrthoSize;
        if (playerRigidbody.velocity.y < 0)
        {
            tgt_OrthoSize = baseOrthoSize + Mathf.Abs(playerRigidbody.velocity.y) * speedMultiplier;
            tgt_OrthoSize = Mathf.Clamp(tgt_OrthoSize, baseOrthoSize, maxOrthoSize);
            
        }
        else
        {
            tgt_OrthoSize = baseOrthoSize;
        }

        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, tgt_OrthoSize, zoomSpeed * Time.deltaTime);
    }
}
