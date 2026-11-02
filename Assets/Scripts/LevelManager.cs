using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// Het centrum van een level. Telt doden, houdt de tijd bij en onthoudt het laatste checkpoint.
/// Gaat één speler dood, dan gaan beide spelers terug naar het checkpoint (of het startpunt).
/// </summary>
public class LevelManager : MonoBehaviour
{
    private const float RespawnDelay = 0.5f;
    private const float PlayerSpacing = 0.6f;

    [SerializeField] private Transform startPoint;

    private int deaths;
    private float elapsedTime;
    private Checkpoint currentCheckpoint;
    private bool isRespawning;
    private ISwapTimer swapTimer;

    public int Deaths => deaths;
    public float ElapsedTime => elapsedTime;

    private void Awake()
    {
        swapTimer = FindObjectsByType<MonoBehaviour>().OfType<ISwapTimer>().FirstOrDefault();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void OnPlayerDied()
    {
        // Beide spelers of meerdere zones tegelijk: maar één keer tellen.
        if (isRespawning)
        {
            return;
        }

        deaths++;
        Debug.Log($"LevelManager: speler dood, deaths = {deaths}.", this);
        StartCoroutine(RespawnAfterDelay());
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        currentCheckpoint = checkpoint;
        Debug.Log($"LevelManager: nieuw checkpoint '{checkpoint.name}'.", this);
        ResetSwapTimer();
    }

    private IEnumerator RespawnAfterDelay()
    {
        isRespawning = true;
        yield return new WaitForSeconds(RespawnDelay);
        Respawn();
        isRespawning = false;
    }

    private void Respawn()
    {
        Vector2 point = currentCheckpoint != null ? currentCheckpoint.transform.position
            : startPoint != null ? startPoint.position
            : transform.position;

        IPlayer[] players = FindObjectsByType<MonoBehaviour>().OfType<IPlayer>().ToArray();
        for (int i = 0; i < players.Length; i++)
        {
            float offset = (i - (players.Length - 1) / 2f) * PlayerSpacing;
            players[i].Respawn(point + new Vector2(offset, 0f));
        }

        ResetSwapTimer();
    }

    private void ResetSwapTimer()
    {
        if (swapTimer != null)
        {
            swapTimer.ResetTimer();
        }
        else
        {
            Debug.Log("LevelManager: wissel-timer reset (nog geen SwapManager in deze scene).", this);
        }
    }

    [ContextMenu("Test: speler dood")]
    private void TestPlayerDied()
    {
        OnPlayerDied();
    }
}
