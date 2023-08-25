using UnityEngine;

public class PositionSync : MonoBehaviour
{
    public Transform sourceParent; // The source parent whose position you want to monitor
    public Transform targetParent; // The target parent whose position you want to update

    private void Update()
    {
        if (sourceParent != null && targetParent != null)
        {
            Vector3 updatedPosition = new Vector3(sourceParent.position.x, sourceParent.position.y, 0);
            targetParent.position = updatedPosition;
        }
    }
}