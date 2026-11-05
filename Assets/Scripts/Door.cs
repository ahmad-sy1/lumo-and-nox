using UnityEngine;

/// <summary>
/// Deur die open en dicht kan. Open: je loopt erdoorheen. Dicht: hij blokkeert.
/// Met openSprite/closedSprite wisselt het plaatje mee; zonder openSprite is de open deur onzichtbaar.
/// Sturen meerdere knoppen of hendels de deur aan, dan blijft hij open zolang er minstens één aanstaat.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Door : Activatable
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    private int activations;

    private void Start()
    {
        SetSolid(!isOpen);
    }

    public override void Activate()
    {
        activations++;
        isOpen = true;
        SetSolid(false);
    }

    public override void Deactivate()
    {
        activations = Mathf.Max(0, activations - 1);
        if (activations > 0)
        {
            return;
        }

        isOpen = false;
        SetSolid(true);
    }

    private void SetSolid(bool solid)
    {
        GetComponent<Collider2D>().enabled = solid;

        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Sprite sprite = solid ? closedSprite : openSprite;

            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = solid;
            }
        }
    }
}
