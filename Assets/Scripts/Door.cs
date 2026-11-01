using UnityEngine;

/// <summary>
/// Deur die open en dicht kan. Open: je loopt erdoorheen en hij is onzichtbaar. Dicht: hij blokkeert.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Door : Activatable
{
    [SerializeField] private bool isOpen;

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
            spriteRenderer.enabled = solid;
        }
    }
}
