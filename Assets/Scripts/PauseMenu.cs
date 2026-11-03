using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Pauzemenu: Esc (beide spelers) zet het spel stil. Verder, Opnieuw (level vanaf het begin),
/// Opties en Hoofdmenu. Tijdens de pauze staat Time.timeScale op 0, dus ook de wissel-timer.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private OptionsMenu optionsMenu;

    private LevelManager levelManager;

    private void Awake()
    {
        panel.SetActive(false);
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            return;
        }

        // Esc in Opties gaat terug naar dit menu, niet verder met spelen.
        if (optionsMenu != null && (optionsMenu.IsOpen || optionsMenu.ClosedThisFrame))
        {
            return;
        }

        if (panel.activeSelf)
        {
            Resume();
        }
        else
        {
            Show();
        }
    }

    public void Show()
    {
        // Na het portaal is het winscherm in beeld; dan geen pauze.
        if (levelManager != null && levelManager.IsFinished)
        {
            return;
        }

        panel.SetActive(true);
        Time.timeScale = 0f;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Level opnieuw vanaf het begin: tijd, kristallen en deaths op 0.
    /// </summary>
    public void Restart()
    {
        GameManager.Instance.ReloadScene();
    }

    public void OpenOptions()
    {
        optionsMenu.Show(panel);
    }

    /// <summary>
    /// Naar het hoofdmenu. De voortgang van dit level vervalt.
    /// </summary>
    public void GoToMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    [ContextMenu("Test: pauze")]
    private void TestPause()
    {
        Show();
    }
}
