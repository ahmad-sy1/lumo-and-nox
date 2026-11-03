using UnityEditor;
using UnityEngine;

/// <summary>
/// Zet de import-instellingen voor pixel art goed voor alles onder Assets/Art/ en Assets/Sprites/:
/// Sprite, 16 pixels per unit (één tegel), scherpe pixels, geen compressie en geen mipmaps.
/// Karakters zijn 24 px en worden zo 1,5 tegel hoog. Een *_sheet.png in Art/ wordt opgedeeld in frames
/// van 16 px breed met de pivot onderaan.
/// </summary>
public class PixelArtImporter : AssetPostprocessor
{
    private const int PixelsPerUnit = 16;
    private const int SheetFrameWidth = 16;

    // Ophogen als de regels hieronder veranderen, dan importeert Unity alles opnieuw.
    public override uint GetVersion()
    {
        return 2;
    }

    // Pixel-fonts scherp houden.
    private void OnPreprocessAsset()
    {
        if (assetPath.StartsWith("Assets/Art/") && assetImporter is TrueTypeFontImporter fontImporter)
        {
            fontImporter.fontRenderingMode = FontRenderingMode.HintedRaster;
        }
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
            if (assetPath.EndsWith("_sheet.png"))
            {
                SliceSheet(importer);
            }
            else
            {
                importer.spriteImportMode = SpriteImportMode.Single;
            }
        }
    }

    private void SliceSheet(TextureImporter importer)
    {
        importer.GetSourceTextureWidthAndHeight(out int width, out int height);
        var frames = new SpriteMetaData[width / SheetFrameWidth];

        for (int i = 0; i < frames.Length; i++)
        {
            frames[i] = new SpriteMetaData
            {
                name = System.IO.Path.GetFileNameWithoutExtension(assetPath) + "_" + i,
                rect = new Rect(i * SheetFrameWidth, 0, SheetFrameWidth, height),
                alignment = (int)SpriteAlignment.BottomCenter,
                pivot = new Vector2(0.5f, 0f)
            };
        }

        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritesheet = frames;
    }
}
