using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uitgangsportaal. Het level is klaar als beide spelers er tegelijk in staan.
/// Staat er maar één in, dan licht het portaal half op.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ExitPortal : MonoBehaviour
{
    private const int PlayersNeeded = 2;

    private readonly HashSet<GameObject> playersInside = new HashSet<GameObject>();
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
            Enter(other.gameObject);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside.Remove(other.gameObject);
            UpdateGlow();
        }
    }

    private void Enter(GameObject player)
    {
        playersInside.Add(player);
        UpdateGlow();

        if (playersInside.Count >= PlayersNeeded && levelManager != null)
        {
            levelManager.FinishLevel();
        }
    }

    private void UpdateGlow()
    {
        // Half licht als er één speler in staat, vol licht als het er twee zijn.
        float brightness = playersInside.Count == 0 ? 0.6f : playersInside.Count == 1 ? 0.8f : 1f;
        GetComponent<SpriteRenderer>().color = new Color(brightness, brightness, brightness);
    }

    [ContextMenu("Test: volgende speler erin")]
    private void TestNextPlayer()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!playersInside.Contains(player))
            {
                Enter(player);
                return;
            }
        }
    }

    [ContextMenu("Test: alle spelers eruit")]
    private void TestEmpty()
    {
        playersInside.Clear();
        UpdateGlow();
    }
}
