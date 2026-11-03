using System;
using UnityEngine;

/// <summary>
/// Telt af tot de wissel en laat dan beide spelers van vorm wisselen (licht &lt;-&gt; schaduw).
/// Drie seconden vooraf klinkt een piep en gaat IsWarning aan: de HUD maakt de timerbalk dan rood en laat
/// het scherm knipperen als FlashWarning aan staat. De timer loopt op Time.deltaTime, dus hij staat stil
/// tijdens de pauze en als het level klaar is. De LevelManager reset hem via ISwapTimer.
/// </summary>
public class SwapManager : MonoBehaviour, ISwapTimer
{
    private const int BeepSampleRate = 44100;

    [SerializeField] private float roundTime = 30f;
    [SerializeField] private float warningTime = 3f;

    private float timeLeft;
    private bool warned;
    private Player[] players;
    private AudioSource audioSource;

    public float RoundTime => roundTime;

    public float TimeLeft => timeLeft;

    public bool IsWarning => warned;

    /// <summary>
    /// Mag het scherm knipperen bij de waarschuwing? Dat staat in Opties (Rustig of Uit).
    /// </summary>
    public bool FlashWarning => GameManager.Instance.Data.settings.flashWarning;

    /// <summary>
    /// Gaat af als de waarschuwing begint. De HUD luistert hiernaar.
    /// </summary>
    public event Action WarningStarted;

    /// <summary>
    /// Gaat af nadat beide spelers van vorm zijn gewisseld. De HUD wisselt dan de vorm-iconen.
    /// </summary>
    public event Action Swapped;

    private void Awake()
    {
        timeLeft = roundTime;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = CreateBeep();
    }

    private void Start()
    {
        players = FindObjectsByType<Player>();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (!warned && timeLeft <= warningTime)
        {
            Warn();
        }

        if (timeLeft <= 0f)
        {
            Swap();
        }
    }

    /// <summary>
    /// Start de timer opnieuw: bij een checkpoint en na een respawn (door de LevelManager).
    /// </summary>
    public void ResetTimer()
    {
        timeLeft = roundTime;
        warned = false;
    }

    private void Warn()
    {
        warned = true;
        Debug.Log("SwapManager: waarschuwing, wissel over " + Mathf.CeilToInt(timeLeft) + " s", this);
        audioSource.volume = GameManager.Instance.Data.settings.sfxVolume;
        audioSource.Play();
        WarningStarted?.Invoke();
    }

    private void Swap()
    {
        foreach (Player player in players)
        {
            player.SetShape(player.CurrentShape == Shape.Light ? Shape.Shadow : Shape.Light);
        }

        Debug.Log("SwapManager: gewisseld", this);
        ResetTimer();
        Swapped?.Invoke();
    }

    /// <summary>
    /// Twee korte, duidelijke pieptonen achter elkaar, zodat we geen geluidsbestand nodig hebben.
    /// </summary>
    private static AudioClip CreateBeep()
    {
        const float beepLength = 0.18f;
        const float gapLength = 0.08f;
        const float frequency = 1000f;
        const float loudness = 0.9f;
        const float edge = 0.01f;

        int beepSamples = (int)(BeepSampleRate * beepLength);
        int gapSamples = (int)(BeepSampleRate * gapLength);
        int edgeSamples = (int)(BeepSampleRate * edge);

        var samples = new float[beepSamples * 2 + gapSamples];
        for (int beep = 0; beep < 2; beep++)
        {
            int start = beep * (beepSamples + gapSamples);
            for (int i = 0; i < beepSamples; i++)
            {
                // Korte in- en uitfade tegen een tik aan het begin en einde.
                float fade = Mathf.Min(1f, Mathf.Min(i, beepSamples - i) / (float)edgeSamples);
                samples[start + i] = Mathf.Sin(2f * Mathf.PI * frequency * i / BeepSampleRate) * loudness * fade;
            }
        }

        AudioClip clip = AudioClip.Create("Piep", samples.Length, 1, BeepSampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    [ContextMenu("Test: wissel nu")]
    private void TestSwap()
    {
        Swap();
    }
}
