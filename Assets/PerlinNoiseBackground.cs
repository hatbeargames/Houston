using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PerlinNoiseBackground : MonoBehaviour
{
    public Color color1 = Color.white;
    public Color color2 = Color.black;

    public float scrollSpeed = 0.5f;
    private MeshRenderer _meshRenderer;
    public Transform character; // Assign your character's transform here
    private Vector2 initialOffset;
    private void Start()
    {
        character = GameObject.Find("SpaceCraft").transform;
        _meshRenderer = GetComponent<MeshRenderer>();
        initialOffset = new Vector2(character.position.x, character.position.y);
        // Generate the tileable texture
        int textureWidth = Mathf.NextPowerOfTwo(Screen.width);
        int textureHeight = Mathf.NextPowerOfTwo(Screen.height);
        Texture2D texture = GenerateTileablePerlinNoiseTexture(textureWidth, textureHeight, new Color[] { color1, color2 });

        // Assign the generated texture to the material of the MeshRenderer
        if (_meshRenderer.materials.Length > 0)
        {
            _meshRenderer.materials[0].mainTexture = texture;
            _meshRenderer.materials[0].mainTextureScale = new Vector2(Screen.width / (float)textureWidth, Screen.height / (float)textureHeight);
        }
    }
    private void Update()
    {
        Vector2 movement = (new Vector2(character.position.x, character.position.y) - initialOffset) * scrollSpeed;
        _meshRenderer.materials[0].mainTextureOffset = movement;
    }
    private Texture2D GenerateTileablePerlinNoiseTexture(int width, int height, Color[] colors)
    {
        Texture2D texture = new Texture2D(width, height);

        float xOffset = Random.Range(0f, 9999f);
        float yOffset = Random.Range(0f, 9999f);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                // Use sine and cosine to get values that repeat at the start and end of the 0-1 interval
                float pX = (x * 1.0f / width) * 2 * Mathf.PI;
                float pY = (y * 1.0f / height) * 2 * Mathf.PI;

                float sampleX = Mathf.PerlinNoise(xOffset + Mathf.Cos(pX), xOffset + Mathf.Sin(pX));
                float sampleY = Mathf.PerlinNoise(yOffset + Mathf.Cos(pY), yOffset + Mathf.Sin(pY));

                // Blend both samples for smoother noise
                float sample = (sampleX + sampleY) * 0.5f;

                Color color = Color.Lerp(colors[0], colors[1], sample);
                texture.SetPixel(x, y, color);
            }
        }

        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();
        return texture;
    }


}
