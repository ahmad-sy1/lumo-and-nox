using System;

/// <summary>
/// Voortgang van één level in save.json. bestTime in seconden (0 = nog niet gehaald).
/// Publieke velden, want JsonUtility slaat die op (zie het data-model).
/// </summary>
[Serializable]
public class LevelProgress
{
    public int levelIndex;
    public bool completed;
    public float bestTime;
    public int crystalsFound;
}
