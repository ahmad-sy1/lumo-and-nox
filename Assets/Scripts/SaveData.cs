using System;

/// <summary>
/// Alles wat we opslaan in save.json: de voortgang per level en de instellingen.
/// </summary>
[Serializable]
public class SaveData
{
    public LevelProgress[] levels;
    public Settings settings = new Settings();

    public static SaveData CreateDefault(int levelCount)
    {
        var data = new SaveData { levels = new LevelProgress[levelCount] };
        for (int i = 0; i < levelCount; i++)
        {
            data.levels[i] = new LevelProgress { levelIndex = i };
        }
        return data;
    }
}
