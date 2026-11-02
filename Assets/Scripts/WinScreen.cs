using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Winscherm na een level: tijd, kristallen en aantal keer doodgegaan, met de knoppen
/// Volgend level, Opnieuw en Hoofdmenu. Na het laatste level wordt Volgend level "Terug naar hoofdmenu".
/// </summary>
public class WinScreen : MonoBehaviour
{
    private const float MissingCrystalAlpha = 0.2f;

    [SerializeField] private GameObject panel;
    [SerializeField] private Text levelText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text crystalText;
    [SerializeField] private Text deathText;
    [SerializeField] private Image[] crystalIcons;
    [SerializeField] private Button nextButton;

    private int levelIndex;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(int levelIndex, float time, int crystals, int deaths)
    {
        this.levelIndex = levelIndex;
        panel.SetActive(true);

        levelText.text = GameManager.LevelNames[levelIndex];
        timeText.text = GameManager.FormatTime(time);
        crystalText.text = $"{crystals}/{crystalIcons.Length}";
        deathText.text = deaths.ToString();

        for (int i = 0; i < crystalIcons.Length; i++)
        {
            crystalIcons[i].color = new Color(1f, 1f, 1f, i < crystals ? 1f : MissingCrystalAlpha);
        }

        nextButton.GetComponentInChildren<Text>().text = IsLastLevel ? "Terug naar hoofdmenu" : "Volgend level";
        EventSystem.current?.SetSelectedGameObject(nextButton.gameObject);
    }

    public bool IsShown => panel.activeSelf;

    private bool IsLastLevel => levelIndex >= GameManager.LevelCount - 1;

    public void NextLevel()
    {
        if (IsLastLevel)
        {
            GameManager.Instance.LoadMainMenu();
        }
        else
        {
            GameManager.Instance.LoadLevel(levelIndex + 1);
        }
    }

    public void Restart()
    {
        GameManager.Instance.ReloadScene();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    [ContextMenu("Test: tonen na Level 1")]
    private void TestShowLevel1()
    {
        Show(0, 83.4f, 2, 3);
    }

    [ContextMenu("Test: tonen na Level 2")]
    private void TestShowLevel2()
    {
        Show(1, 125f, 3, 0);
    }
}
