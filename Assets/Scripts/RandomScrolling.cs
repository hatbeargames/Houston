using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomScrolling : MonoBehaviour
{
    public float minXSpeed = 0.01f;
    public float maxXSpeed = 0.05f;
    public float minYSpeed = 0.01f;
    public float maxYSpeed = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Vector2 scrollSpeed;
    private Vector2 currentOffset;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null || spriteRenderer.sharedMaterial == null)
        {
            Debug.LogError("No material found for random scrolling.");
            return;
        }

        // Get the initial offset (might be set in the editor)
        currentOffset = spriteRenderer.sharedMaterial.GetTextureOffset("_MainTex");

        // Assign random speeds
        scrollSpeed = new Vector2(
            Random.Range(minXSpeed, maxXSpeed),
            Random.Range(minYSpeed, maxYSpeed)
        );
    }

    private void Update()
    {
        if (spriteRenderer == null || spriteRenderer.sharedMaterial == null) return;

        currentOffset += scrollSpeed * Time.deltaTime;

        // Avoid floating point precision issues
        currentOffset.x = Mathf.Repeat(currentOffset.x, 1);
        currentOffset.y = Mathf.Repeat(currentOffset.y, 1);

        spriteRenderer.sharedMaterial.SetTextureOffset("_MainTex", currentOffset);
    }
}
