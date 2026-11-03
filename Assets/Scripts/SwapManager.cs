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

        ResetTimer();
        Swapped?.Invoke();
    }

    /// <summary>
    /// Een korte piep, zodat we geen geluidsbestand nodig hebben.
    /// </summary>
    private static AudioClip CreateBeep()
    {
        const float duration = 0.2f;
        const float frequency = 880f;

        var samples = new float[(int)(BeepSampleRate * duration)];
        for (int i = 0; i < samples.Length; i++)
        {
            float fade = 1f - (float)i / samples.Length;
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / BeepSampleRate) * 0.5f * fade;
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
