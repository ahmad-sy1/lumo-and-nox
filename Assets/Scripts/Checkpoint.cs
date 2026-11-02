using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lantaarn als checkpoint. Hij gaat pas branden als beide spelers erlangs zijn gelopen.
/// Daarna is hij het nieuwe respawnpunt en start de wissel-timer opnieuw.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour
{
    private const int PlayersNeeded = 2;

    [SerializeField] private Sprite litSprite;

    private readonly HashSet<GameObject> passedPlayers = new HashSet<GameObject>();
    private bool isActive;
    private LevelManager levelManager;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pass(other.gameObject);
        }
    }

    private void Pass(GameObject player)
    {
        if (isActive)
        {
            return;
        }

        passedPlayers.Add(player);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (passedPlayers.Count < PlayersNeeded)
        {
            // Gloeit zwak: wacht op de andere speler.
            spriteRenderer.color = Palette.gloed;
            Debug.Log($"{name}: {player.name} is erlangs, wacht op de andere speler.", this);
            return;
        }

        isActive = true;
        spriteRenderer.sprite = litSprite != null ? litSprite : spriteRenderer.sprite;
        spriteRenderer.color = Color.white;

        if (levelManager != null)
        {
            levelManager.SetCheckpoint(this);
        }
    }

    [ContextMenu("Test: volgende speler erlangs")]
    private void TestNextPlayer()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!passedPlayers.Contains(player))
            {
                Pass(player);
                return;
            }
        }
    }
}
