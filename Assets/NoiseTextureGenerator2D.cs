using UnityEngine;

public class NoiseTextureGenerator2D : MonoBehaviour
{
    public int textureWidth = 256; // Now public
    public int textureHeight = 256; // Now public
    public float frequency = 5.0f;
    public SpriteRenderer targetSpriteRenderer; // Drag your SpriteRenderer here.

    void Start()
    {
        GenerateNoiseTexture();
    }

    void GenerateNoiseTexture()
    {
        Texture2D noiseTexture = new Texture2D(textureWidth, textureHeight);

        for (int y = 0; y < noiseTexture.height; y++)
        {
            for (int x = 0; x < noiseTexture.width; x++)
            {
                float pX = (float)x / noiseTexture.width;
                float pY = (float)y / noiseTexture.height;

                float sample = Mathf.PerlinNoise(pX * frequency, pY * frequency);
                noiseTexture.SetPixel(x, y, new Color(sample, sample, sample));
            }
        }

        noiseTexture.Apply();
        targetSpriteRenderer.sprite = Sprite.Create(noiseTexture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));
    }
}
