using System.Collections;
using UnityEngine;

public class DelayedActivation : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject; // Drag the GameObject you want to enable in the Unity inspector

    private void Start()
    {
        StartCoroutine(WaitAndActivate());
    }

    IEnumerator WaitAndActivate()
    {
        yield return new WaitForSeconds(.5f); // Wait for 5 seconds

        if (targetObject != null)
        {
            targetObject.SetActive(true); // Enable the GameObject
        }
    }
}
