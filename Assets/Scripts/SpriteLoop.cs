using UnityEngine;

/// <summary>
/// Speelt een rij plaatjes in een lus af. Gebruikt Time.time, dus hij staat stil tijdens de pauze.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLoop : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 6f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0)
        {
            return;
        }

        spriteRenderer.sprite = frames[(int)(Time.time * framesPerSecond) % frames.Length];
    }
}
