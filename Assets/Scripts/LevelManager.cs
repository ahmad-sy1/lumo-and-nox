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
    private const int MaxCrystals = 3;

    [SerializeField] private Transform startPoint;

    private int deaths;
    private float elapsedTime;
    private int crystals;
    private bool isFinished;
    private Checkpoint currentCheckpoint;
    private bool isRespawning;
    private ISwapTimer swapTimer;

    public int Deaths => deaths;
    public float ElapsedTime => elapsedTime;
    public int Crystals => crystals;

    private void Awake()
    {
        swapTimer = FindObjectsByType<MonoBehaviour>().OfType<ISwapTimer>().FirstOrDefault();
    }

    private void Update()
    {
        if (!isFinished)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void OnPlayerDied()
    {
        // Beide spelers of meerdere zones tegelijk: maar één keer tellen.
        if (isRespawning || isFinished)
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

    public void AddCrystal()
    {
        crystals = Mathf.Min(crystals + 1, MaxCrystals);
        Debug.Log($"LevelManager: kristallen {crystals}/{MaxCrystals}.", this);
    }

    public void FinishLevel()
    {
        if (isFinished)
        {
            return;
        }

        // Alles stilzetten: besturing, wissel-timer en tijd. Het winscherm zet timeScale weer terug.
        isFinished = true;
        Time.timeScale = 0f;
        Debug.Log($"LevelManager: level klaar! Tijd {elapsedTime:0.0} s, kristallen {crystals}/{MaxCrystals}, deaths {deaths}.", this);

        // Het winscherm (issue #25) roept hier straks WinScreen.Show(elapsedTime, crystals, deaths) aan.
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

    [ContextMenu("Test: level klaar")]
    private void TestFinishLevel()
    {
        FinishLevel();
    }

    [ContextMenu("Test: speler dood")]
    private void TestPlayerDied()
    {
        OnPlayerDied();
    }
}
