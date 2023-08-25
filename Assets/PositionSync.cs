using UnityEngine;

public class PositionSync : MonoBehaviour
{
    public Transform sourceParent; // The source parent whose position you want to monitor
    public Transform targetParent; // The target parent whose position you want to update

    private void Update()
    {
        if (sourceParent != null && targetParent != null)
        {
            targetParent.position = sourceParent.position;
        }
    }
}