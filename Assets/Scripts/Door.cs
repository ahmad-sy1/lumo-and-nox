using UnityEngine;

/// <summary>
/// Deur die open en dicht kan. Open: je loopt erdoorheen. Dicht: hij blokkeert.
/// Met openSprite/closedSprite wisselt het plaatje mee; zonder openSprite is de open deur onzichtbaar.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Door : Activatable
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;

    private void Start()
    {
        if (isOpen)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public override void Activate()
    {
        isOpen = true;
        SetSolid(false);
    }

    public override void Deactivate()
    {
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
