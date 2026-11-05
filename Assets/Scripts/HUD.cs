using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// De HUD tijdens het spelen: timerbalk (boven in het midden), vorm-icoon van Lumo (linksboven) en Nox
/// (rechtsboven) en kristallen x/3 (rechtsonder). Hij leest alles uit SwapManager, Player en LevelManager.
/// In het pauze- en winscherm staat Time.timeScale op 0; dan verbergt de HUD zichzelf.
/// </summary>
public class HUD : MonoBehaviour
{
    private const float BlinkSpeed = 6f;
    private static readonly Color WarningColor = new Color(0.9f, 0.25f, 0.25f);

    [SerializeField] private GameObject content;
    [SerializeField] private Image timerFill;
    [SerializeField] private Text timerText;
    [SerializeField] private Image lumoIcon;
    [SerializeField] private Image noxIcon;
    [SerializeField] private Text crystalText;
    [SerializeField] private Sprite lightIcon;
    [SerializeField] private Sprite shadowIcon;

    private SwapManager swapManager;
    private LevelManager levelManager;
    private Player lumo;
    private Player nox;

    private void Start()
    {
        swapManager = FindAnyObjectByType<SwapManager>();
        levelManager = FindAnyObjectByType<LevelManager>();

        foreach (Player player in FindObjectsByType<Player>())
        {
            if (player.ControlScheme == Player.Controls.Wasd)
            {
                lumo = player;
            }
            else
            {
                nox = player;
            }
        }
    }

    private void Update()
    {
        content.SetActive(Time.timeScale > 0f);

        UpdateTimer();
        UpdateShapes();
        UpdateCrystals();
    }

    private void UpdateTimer()
    {
        if (swapManager == null)
        {
            return;
        }

        float timeLeft = Mathf.Max(0f, swapManager.TimeLeft);
        timerFill.fillAmount = timeLeft / swapManager.RoundTime;
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();

        // Laatste 3 seconden: rood. Knipperen alleen als dat aan staat in Opties.
        bool warning = swapManager.IsWarning;
        bool visible = !warning || !swapManager.FlashWarning || Mathf.Sin(Time.time * BlinkSpeed) > 0f;
        timerFill.color = warning ? WarningColor : Palette.goud;
        timerFill.enabled = visible;
        timerText.color = warning ? WarningColor : Palette.steenH;
    }

    private void UpdateShapes()
    {
        ShowShape(lumoIcon, lumo);
        ShowShape(noxIcon, nox);
    }

    private void ShowShape(Image icon, Player player)
    {
        if (player != null)
        {
            icon.sprite = player.CurrentShape == Shape.Light ? lightIcon : shadowIcon;
        }
    }

    private void UpdateCrystals()
    {
        if (levelManager != null)
        {
            crystalText.text = $"{levelManager.Crystals}/3";
        }
    }
}
