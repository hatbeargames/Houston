using UnityEngine;

public class SpriteScroller2D : MonoBehaviour
{
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;
    public NoiseTextureGenerator2D NTG2D;
    private Vector3 startPosition;

    void Start()
    {
        NTG2D = GameObject.Find("GameManager").GetComponent<NoiseTextureGenerator2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        float newPositionX = Mathf.Repeat(Time.time * scrollSpeedX, NTG2D.textureWidth); // textureWidth should be the width of your sprite in world units.
        float newPositionY = Mathf.Repeat(Time.time * scrollSpeedY, NTG2D.textureHeight); // textureHeight should be the height of your sprite in world units.
        transform.position = startPosition + new Vector3(newPositionX, newPositionY, 0);
    }
}
