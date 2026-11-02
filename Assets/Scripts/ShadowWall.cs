using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Muur waar alleen een speler met Schaduwstap doorheen kan. De Player roept SetPassable() aan met
/// zijn eigen collider, dus de muur blijft dicht voor de andere speler.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ShadowWall : MonoBehaviour
{
    private const float PassableAlpha = 0.4f;

    private readonly HashSet<Collider2D> passingPlayers = new HashSet<Collider2D>();
    private bool isPassable;

    public void SetPassable(Collider2D player, bool passable)
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), player, passable);

        if (passable)
        {
            passingPlayers.Add(player);
        }
        else
        {
            passingPlayers.Remove(player);
        }

        isPassable = passingPlayers.Count > 0;

        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = spriteRenderer.color;
            color.a = isPassable ? PassableAlpha : 1f;
            spriteRenderer.color = color;
        }
    }

    [ContextMenu("Test: Schaduwstap aan (alle spelers)")]
    private void TestPassableOn()
    {
        SetPassableForAllPlayers(true);
    }

    [ContextMenu("Test: Schaduwstap uit (alle spelers)")]
    private void TestPassableOff()
    {
        SetPassableForAllPlayers(false);
    }

    private void SetPassableForAllPlayers(bool passable)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.TryGetComponent(out Collider2D playerCollider))
            {
                SetPassable(playerCollider, passable);
            }
        }

        Debug.Log($"{name}: doorloopbaar = {isPassable}.", this);
    }
}
