using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Startscherm, hoofdmenu en level select in MainMenu.unity. Werkt met muis of pijltjes + Enter.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private Text pressEnterText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Text[] levelInfoTexts;
    [SerializeField] private OptionsMenu optionsMenu;

    private void Start()
    {
        ShowStart();
    }

    private void Update()
    {
        // Rustig knipperen, geen felle flitsen.
        if (startPanel.activeSelf)
        {
            Color color = pressEnterText.color;
            color.a = 0.4f + 0.6f * Mathf.PingPong(Time.unscaledTime * 0.5f, 1f);
            pressEnterText.color = color;
        }

        if (levelSelectPanel.activeSelf && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ShowMain();
        }
    }

    public void ShowStart()
    {
        ShowPanel(startPanel, startPanel);
    }

    public void ShowMain()
    {
        ShowPanel(mainPanel, playButton.gameObject);
    }

    public void ShowLevelSelect()
    {
        RefreshLevelSelect();
        ShowPanel(levelSelectPanel, levelButtons[0].gameObject);
    }

    public void OpenOptions()
    {
        optionsMenu.Show(mainPanel);
    }

    /// <summary>
    /// Spelen: het eerste level dat nog niet gehaald is.
    /// </summary>
    public void Play()
    {
        GameManager.Instance.LoadLevel(GameManager.Instance.FirstUnfinishedLevel());
    }

    public void SelectLevel(int index)
    {
        if (GameManager.Instance.IsUnlocked(index))
        {
            GameManager.Instance.LoadLevel(index);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void RefreshLevelSelect()
    {
        GameManager gameManager = GameManager.Instance;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool unlocked = gameManager.IsUnlocked(i);
            LevelProgress progress = gameManager.Data.levels[i];

            levelButtons[i].interactable = unlocked;
            foreach (Text text in levelButtons[i].GetComponentsInChildren<Text>())
            {
                text.color = unlocked ? Palette.nachtS : Palette.steenS;
            }

            if (!unlocked)
            {
                levelInfoTexts[i].text = $"Op slot - haal eerst Level {i}";
            }
            else if (progress.completed)
            {
                levelInfoTexts[i].text =
                    $"Gehaald   Beste tijd: {GameManager.FormatTime(progress.bestTime)}   Kristallen: {progress.crystalsFound}/3";
            }
            else
            {
                levelInfoTexts[i].text = "Nog niet gehaald   Beste tijd: -   Kristallen: -";
            }
        }
    }

    private void ShowPanel(GameObject panel, GameObject selected)
    {
        startPanel.SetActive(panel == startPanel);
        mainPanel.SetActive(panel == mainPanel);
        levelSelectPanel.SetActive(panel == levelSelectPanel);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(selected);
        }
    }
}
