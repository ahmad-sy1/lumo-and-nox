using UnityEngine;

/// <summary>
/// Het kleurenpalet van Lumo en Nox, gelijk aan dict C in tools/generate_tiles.py.
/// Achtervoegsels: H = highlight (lichter), S = schaduwkant, D = donker.
/// </summary>
public static class Palette
{
    // Licht (goed)
    public static readonly Color nachtL = new Color32(42, 35, 64, 255);
    public static readonly Color steen = new Color32(232, 217, 176, 255);
    public static readonly Color steenH = new Color32(247, 238, 212, 255);
    public static readonly Color steenS = new Color32(201, 169, 106, 255);
    public static readonly Color steenD = new Color32(150, 118, 66, 255);
    public static readonly Color goud = new Color32(242, 193, 78, 255);
    public static readonly Color goudD = new Color32(190, 132, 40, 255);
    public static readonly Color gloed = new Color32(255, 233, 168, 255);
    public static readonly Color mos = new Color32(111, 143, 90, 255);
    public static readonly Color mosD = new Color32(77, 107, 63, 255);

    // Schaduw (kwaad)
    public static readonly Color nachtS = new Color32(20, 17, 42, 255);
    public static readonly Color rots = new Color32(59, 47, 92, 255);
    public static readonly Color rotsH = new Color32(82, 66, 124, 255);
    public static readonly Color rotsS = new Color32(42, 33, 71, 255);
    public static readonly Color rotsD = new Color32(28, 22, 50, 255);
    public static readonly Color doorn = new Color32(107, 79, 160, 255);
    public static readonly Color doornH = new Color32(150, 118, 206, 255);
    public static readonly Color poel = new Color32(11, 9, 24, 255);

    // Gedeeld
    public static readonly Color kristal = new Color32(143, 211, 255, 255);
    public static readonly Color kristalH = new Color32(201, 240, 255, 255);
    public static readonly Color kristalD = new Color32(74, 138, 196, 255);
    public static readonly Color hout = new Color32(122, 86, 58, 255);
    public static readonly Color houtH = new Color32(158, 116, 78, 255);
    public static readonly Color houtD = new Color32(82, 56, 40, 255);
    public static readonly Color ijzer = new Color32(92, 96, 120, 255);
    public static readonly Color ijzerH = new Color32(140, 146, 170, 255);
}
