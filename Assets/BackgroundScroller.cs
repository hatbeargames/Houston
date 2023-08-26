using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private MeshRenderer _meshRenderer;
    public Transform character; // Assign your character's transform here
    private Vector2 initialOffset;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        // It's good practice to null-check in case the character is not assigned or the GameObject isn't found.
        if (character == null)
        {
            character = GameObject.Find("SpaceCraft").transform;
        }

        if (character)
        {
            initialOffset = new Vector2(character.position.x, character.position.y);
        }
    }

    private void Update()
    {
        if (character && _meshRenderer && _meshRenderer.materials.Length > 0)
        {
            Vector2 movement = (new Vector2(character.position.x, character.position.y) - initialOffset) * scrollSpeed;
            _meshRenderer.materials[0].mainTextureOffset = movement;
        }
    }
}

