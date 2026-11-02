using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basis voor een gevaarlijke plek. Een speler die erin staat en een gevaarlijke vorm heeft, gaat dood.
/// We checken elke physics-stap opnieuw, dus ook direct na een wissel terwijl de speler stilstaat.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Zone : MonoBehaviour
{
    private readonly HashSet<Collider2D> playersInside = new HashSet<Collider2D>();

    public abstract bool IsDangerousFor(Shape shape);

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<IPlayer>() != null)
        {
            playersInside.Add(other);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        playersInside.Remove(other);
    }

    private void OnDisable()
    {
        playersInside.Clear();
    }

    private void FixedUpdate()
    {
        // Kopie, want Die() kan de speler verplaatsen.
        foreach (Collider2D other in new List<Collider2D>(playersInside))
        {
            IPlayer player = other != null ? other.GetComponentInParent<IPlayer>() : null;

            if (player != null && IsDangerousFor(player.CurrentShape))
            {
                player.Die();
            }
        }
    }

    [ContextMenu("Test: speler licht erin")]
    private void TestLightPlayer()
    {
        MovePlayerInside(Shape.Light);
    }

    [ContextMenu("Test: speler schaduw erin")]
    private void TestShadowPlayer()
    {
        MovePlayerInside(Shape.Shadow);
    }

    private void MovePlayerInside(Shape shape)
    {
        Debug.Log($"{name}: {shape} is hier {(IsDangerousFor(shape) ? "dood" : "veilig")}.", this);

        foreach (PlayerDummy dummy in FindObjectsByType<PlayerDummy>())
        {
            if (dummy.CurrentShape == shape)
            {
                dummy.transform.position = GetComponent<Collider2D>().bounds.center;
                return;
            }
        }
    }
}
