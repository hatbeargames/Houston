using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RenderQueueSetter : MonoBehaviour
{
    public int renderQueueValue = 2000;
    private MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        if (_meshRenderer && _meshRenderer.materials.Length > 0)
        {
            Material targetMaterial = _meshRenderer.materials[0];
            if (targetMaterial)
            {
                targetMaterial.renderQueue = renderQueueValue;
            }
        }
    }
}
