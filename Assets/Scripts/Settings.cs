using System;

/// <summary>
/// Instellingen in save.json. Volumes van 0 t/m 1. flashWarning true = "Rustig", false = "Uit".
/// </summary>
[Serializable]
public class Settings
{
    public float musicVolume = 0.8f;
    public float sfxVolume = 0.8f;
    public bool fullscreen = true;
    public bool flashWarning = true;
}
