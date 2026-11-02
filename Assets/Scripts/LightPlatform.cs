using UnityEngine;

/// <summary>
/// Platform dat alleen zichtbaar en betreedbaar is als een speler in licht-vorm Gloed gebruikt.
/// De Player roept SetRevealed() aan voor platforms in de buurt.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class LightPlatform : MonoBehaviour
{
    [SerializeField] private bool isRevealed;
    [SerializeField] private Sprite revealedSprite;
    [SerializeField] private Sprite hiddenSprite;

    private void Start()
    {
        SetRevealed(isRevealed);
    }

    public void SetRevealed(bool revealed)
    {
        isRevealed = revealed;
        GetComponent<BoxCollider2D>().enabled = revealed;

        Sprite sprite = revealed ? revealedSprite : hiddenSprite;
        if (sprite == null)
        {
            return;
        }

        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sprite = sprite;
        }
    }

    [ContextMenu("Test: Gloed aan")]
    private void TestReveal()
    {
        SetRevealed(true);
    }

    [ContextMenu("Test: Gloed uit")]
    private void TestHide()
    {
        SetRevealed(false);
    }
}
