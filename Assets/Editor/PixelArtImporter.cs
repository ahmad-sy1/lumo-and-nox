using UnityEditor;
using UnityEngine;

/// <summary>
/// Zet de import-instellingen voor pixel art goed voor alles onder Assets/Art/ en Assets/Sprites/:
/// Sprite, 16 pixels per unit (één tegel), scherpe pixels, geen compressie en geen mipmaps.
/// Karakters zijn 24 px en worden zo 1,5 tegel hoog.
/// </summary>
public class PixelArtImporter : AssetPostprocessor
{
    private const int PixelsPerUnit = 16;

    // Ophogen als de regels hieronder veranderen, dan importeert Unity alles opnieuw.
    public override uint GetVersion()
    {
        return 1;
    }

    private void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith("Assets/Art/") && !assetPath.StartsWith("Assets/Sprites/"))
        {
            return;
        }

        TextureImporter importer = (TextureImporter)assetImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = PixelsPerUnit;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.mipmapEnabled = false;

        // Tegels in Art/ zijn losse plaatjes. De karakter-sheets in Sprites/ zijn opgedeeld, die laten we zo.
        if (assetPath.StartsWith("Assets/Art/"))
        {
            importer.spriteImportMode = SpriteImportMode.Single;
        }
    }
}
