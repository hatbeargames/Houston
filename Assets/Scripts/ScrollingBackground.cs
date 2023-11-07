using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speed = 1.0f;
    public Color color1 = Color.white;
    public Color color2 = Color.black;
    public SpriteRenderer backgroundNegative1;
    public SpriteRenderer background0;
    public SpriteRenderer background1;
    public SpriteRenderer background2;

    private float backgroundWidth;

    private void Start()
    {
        Texture2D texture = GenerateTileablePerlinNoiseTexture(256, 256, new Color[] { color1, color2 });
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f), 256);

        backgroundNegative1.sprite = sprite;
        background0.sprite = sprite;
        background1.sprite = sprite;
        background2.sprite = sprite;

        ScaleToCameraViewport(backgroundNegative1);
        ScaleToCameraViewport(background0);
        ScaleToCameraViewport(background1);
        ScaleToCameraViewport(background2);

        backgroundWidth = background1.bounds.size.x;

        // Start with background1 centered
        background1.transform.position = Vector3.zero;
        // Adjust the other backgrounds relative to background1
        background0.transform.position = new Vector3(background1.transform.position.x - backgroundWidth, 0, 0);
        backgroundNegative1.transform.position = new Vector3(background0.transform.position.x - backgroundWidth, 0, 0);
        background2.transform.position = new Vector3(background1.transform.position.x + backgroundWidth, 0, 0);
    }

    private void ScaleToCameraViewport(SpriteRenderer sr)
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;

        Vector3 scale;
        scale.y = height / sr.sprite.bounds.size.y;
        scale.x = width / sr.sprite.bounds.size.x;
        scale.z = 1f;
        sr.transform.localScale = scale;
    }

    private Texture2D GenerateTileablePerlinNoiseTexture(int width, int height, Color[] colors)
    {
        Texture2D texture = new Texture2D(width, height);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float pX = x * 1.0f / width;
                float pY = y * 1.0f / height;

                float sample = Mathf.PerlinNoise(pX, pY);
                Color color = Color.Lerp(colors[0], colors[1], sample);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private void Update()
    {
        Vector3 moveAmount = Vector3.left * speed * Time.deltaTime;
        backgroundNegative1.transform.position += moveAmount;
        background0.transform.position += moveAmount;
        background1.transform.position += moveAmount;
        background2.transform.position += moveAmount;

        CheckAndRepositionBackground(backgroundNegative1, background2);
        CheckAndRepositionBackground(background0, backgroundNegative1);
        CheckAndRepositionBackground(background1, background0);
        CheckAndRepositionBackground(background2, background1);
    }

    private void CheckAndRepositionBackground(SpriteRenderer bgToCheck, SpriteRenderer referenceBg)
    {
        if (bgToCheck.transform.position.x < -backgroundWidth)
        {
            bgToCheck.transform.position = new Vector3(referenceBg.transform.position.x + backgroundWidth, bgToCheck.transform.position.y, bgToCheck.transform.position.z);
        }
    }
}
