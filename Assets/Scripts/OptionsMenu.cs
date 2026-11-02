using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Optiesscherm, te openen vanuit het hoofdmenu en het pauzemenu. Elke wijziging slaan we meteen
/// op in save.json. Terug (of Esc) gaat naar het scherm waar je vandaan kwam.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle flashToggle;

    private GameObject cameFrom;
    private int closedFrame = -1;

    public bool IsOpen => panel.activeSelf;

    /// <summary>
    /// True als de opties in dit frame gesloten zijn. Dan reageert het pauzemenu niet ook nog op dezelfde Esc.
    /// </summary>
    public bool ClosedThisFrame => closedFrame == Time.frameCount;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Back();
        }
    }

    public void Show(GameObject cameFrom)
    {
        this.cameFrom = cameFrom;
        cameFrom.SetActive(false);

        // Huidige instellingen tonen zonder ze opnieuw op te slaan.
        Settings settings = GameManager.Instance.Data.settings;
        musicSlider.SetValueWithoutNotify(settings.musicVolume);
        sfxSlider.SetValueWithoutNotify(settings.sfxVolume);
        fullscreenToggle.SetIsOnWithoutNotify(settings.fullscreen);
        flashToggle.SetIsOnWithoutNotify(settings.flashWarning);
        UpdateFlashLabel();

        panel.SetActive(true);
        Select(musicSlider.gameObject);
    }

    public void OnSettingChanged()
    {
        Settings settings = GameManager.Instance.Data.settings;
        settings.musicVolume = musicSlider.value;
        settings.sfxVolume = sfxSlider.value;
        settings.fullscreen = fullscreenToggle.isOn;
        settings.flashWarning = flashToggle.isOn;
        UpdateFlashLabel();

        GameManager.Instance.SaveSettings();
        GameManager.Instance.ApplySettings();
    }

    public void Back()
    {
        panel.SetActive(false);
        closedFrame = Time.frameCount;

        if (cameFrom != null)
        {
            cameFrom.SetActive(true);
            Selectable first = cameFrom.GetComponentInChildren<Selectable>();
            Select(first != null ? first.gameObject : null);
        }
    }

    private void UpdateFlashLabel()
    {
        flashToggle.GetComponentInChildren<Text>().text = flashToggle.isOn ? "Rustig" : "Uit";
    }

    private static void Select(GameObject selected)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(selected);
        }
    }
}
