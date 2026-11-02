using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Blijft bestaan tussen scenes. Laadt en bewaart save.json (voortgang + instellingen) en laadt levels.
/// GameManager.Instance maakt zichzelf aan als hij nog niet in de scene staat.
/// </summary>
public class GameManager : MonoBehaviour
{
    public const string MainMenuScene = "MainMenu";
    public static readonly string[] LevelNames = { "Level 1 – Het Nachtbos", "Level 2 – De Vuurtoren" };
    private static readonly string[] LevelScenes = { "Level1", "Level2" };

    private static GameManager instance;

    private SaveData data;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    public static int LevelCount => LevelScenes.Length;

    public SaveData Data => data;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadProgress();
        ApplySettings();
    }

    public void LoadLevel(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(LevelScenes[index]);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadProgress()
    {
        data = SaveData.CreateDefault(LevelCount);

        if (!File.Exists(SavePath))
        {
            return;
        }

        try
        {
            // Ontbreekt er iets in het bestand, dan blijven de standaardwaarden staan.
            JsonUtility.FromJsonOverwrite(File.ReadAllText(SavePath), data);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"save.json is kapot, we gebruiken standaardwaarden: {e.Message}", this);
            data = SaveData.CreateDefault(LevelCount);
        }

        if (data.levels == null || data.levels.Length != LevelCount)
        {
            data.levels = SaveData.CreateDefault(LevelCount).levels;
        }
    }

    /// <summary>
    /// Level gehaald. Beste tijd en kristallen overschrijven we alleen als ze beter zijn.
    /// </summary>
    public void SaveProgress(int levelIndex, float time, int crystals)
    {
        LevelProgress level = data.levels[levelIndex];
        level.completed = true;

        if (level.bestTime <= 0f || time < level.bestTime)
        {
            level.bestTime = time;
        }
        level.crystalsFound = Mathf.Max(level.crystalsFound, crystals);

        WriteFile();
    }

    public void SaveSettings()
    {
        WriteFile();
    }

    public void ApplySettings()
    {
        Screen.fullScreen = data.settings.fullscreen;
        // Muziek- en effectvolume gebruiken we zodra er geluid is.
    }

    /// <summary>
    /// Tijd als mm:ss.
    /// </summary>
    public static string FormatTime(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);
        return $"{total / 60:00}:{total % 60:00}";
    }

    public bool IsUnlocked(int levelIndex)
    {
        return levelIndex == 0 || data.levels[levelIndex - 1].completed;
    }

    /// <summary>
    /// Het eerste level dat nog niet gehaald is. Alles gehaald: het laatste level.
    /// </summary>
    public int FirstUnfinishedLevel()
    {
        for (int i = 0; i < LevelCount; i++)
        {
            if (!data.levels[i].completed)
            {
                return i;
            }
        }
        return LevelCount - 1;
    }

    private void WriteFile()
    {
        File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
    }

    [ContextMenu("Test: save.json tonen")]
    private void TestShowSave()
    {
        Debug.Log($"Level 2 open: {IsUnlocked(1)}, Spelen start level {FirstUnfinishedLevel() + 1}.\n{SavePath}\n{JsonUtility.ToJson(data, true)}", this);
    }

    [ContextMenu("Test: Level 1 gehaald (60 s, 2 kristallen)")]
    private void TestLevel1Slow()
    {
        SaveProgress(0, 60f, 2);
        TestShowSave();
    }

    [ContextMenu("Test: Level 1 gehaald (90 s, 3 kristallen)")]
    private void TestLevel1Slower()
    {
        SaveProgress(0, 90f, 3);
        TestShowSave();
    }

    [ContextMenu("Test: save.json wissen")]
    private void TestDeleteSave()
    {
        File.Delete(SavePath);
        LoadProgress();
        TestShowSave();
    }
}
