using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// Bouwt uGUI-onderdelen in de stijl van Lumo en Nox (palet + pixel-font), voor LumoTools.
/// Posities zijn in pixels op een scherm van 1920 x 1080, gemeten vanaf het midden van de ouder.
/// </summary>
public static class UiBuilder
{
    public const int SmallText = 32;
    public const int NormalText = 40;
    public const int LargeText = 64;
    public const int TitleText = 112;

    public const string FontRegular = "Assets/Art/Fonts/Silkscreen-Regular.ttf";
    private const string FontBold = "Assets/Art/Fonts/Silkscreen-Bold.ttf";
    private const string FontTitle = "Assets/Art/Fonts/PressStart2P-Regular.ttf";

    public static Canvas CreateCanvas(string name, int sortingOrder)
    {
        var go = new GameObject(name, typeof(RectTransform));
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        CanvasScaler scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        go.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    public static void CreateEventSystem()
    {
        var go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
        go.AddComponent<InputSystemUIInputModule>();
    }

    /// <summary>
    /// Vlak over de hele ouder.
    /// </summary>
    public static GameObject CreateFullPanel(Transform parent, string name, Color color)
    {
        GameObject panel = CreateImage(parent, name, color, Vector2.zero, Vector2.zero);
        var rect = (RectTransform)panel.transform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        return panel;
    }

    public static GameObject CreateImage(Transform parent, string name, Color color, Vector2 position, Vector2 size)
    {
        GameObject go = CreateRect(parent, name, position, size);
        go.AddComponent<Image>().color = color;
        return go;
    }

    public static Text CreateText(Transform parent, string name, string text, int fontSize, Color color,
        Vector2 position, Vector2 size, TextAnchor alignment = TextAnchor.MiddleCenter, bool bold = false)
    {
        GameObject go = CreateRect(parent, name, position, size);
        Text label = go.AddComponent<Text>();
        label.font = AssetDatabase.LoadAssetAtPath<Font>(bold ? FontBold : FontRegular);
        label.text = text;
        label.fontSize = fontSize;
        label.color = color;
        label.alignment = alignment;
        label.horizontalOverflow = HorizontalWrapMode.Wrap;
        label.verticalOverflow = VerticalWrapMode.Overflow;
        label.raycastTarget = false;
        return label;
    }

    /// <summary>
    /// Titel in Press Start 2P (Silkscreen tekent "&amp;" bijna als een euroteken).
    /// </summary>
    public static Text CreateTitle(Transform parent, string name, string text, int fontSize, Color color,
        Vector2 position, Vector2 size)
    {
        Text title = CreateText(parent, name, text, fontSize, color, position, size);
        title.font = AssetDatabase.LoadAssetAtPath<Font>(FontTitle);
        return title;
    }

    /// <summary>
    /// Knop: beige met donkere tekst, goud als hij geselecteerd is (muis of pijltjes).
    /// </summary>
    public static Button CreateButton(Transform parent, string name, string label, Vector2 position, Vector2 size,
        UnityAction onClick = null)
    {
        GameObject go = CreateImage(parent, name, Color.white, position, size);
        Button button = go.AddComponent<Button>();
        button.colors = ButtonColors();
        // Eén regel hoog, zodat best fit lange tekst kleiner maakt in plaats van af te breken.
        Text text = CreateText(go.transform, "Label", label, NormalText, Palette.nachtS, Vector2.zero, new Vector2(size.x - 32f, NormalText + 8f), TextAnchor.MiddleCenter, true);
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = SmallText / 2;
        text.resizeTextMaxSize = NormalText;

        if (onClick != null)
        {
            UnityEventTools.AddPersistentListener(button.onClick, onClick);
        }
        return button;
    }

    public static void AddClick(Button button, UnityAction<int> onClick, int value)
    {
        UnityEventTools.AddIntPersistentListener(button.onClick, onClick, value);
    }

    public static Slider CreateSlider(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject go = DefaultControls.CreateSlider(DefaultResources());
        go.name = name;
        Place(go, parent, position, size);

        go.transform.Find("Background").GetComponent<Image>().color = Palette.rotsD;
        go.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Palette.goud;
        go.transform.Find("Handle Slide Area/Handle").GetComponent<Image>().color = Palette.steenH;

        Slider slider = go.GetComponent<Slider>();
        slider.colors = ButtonColors();
        slider.value = 0.8f;
        return slider;
    }

    public static Toggle CreateToggle(Transform parent, string name, string label, Vector2 position, Vector2 size)
    {
        GameObject go = DefaultControls.CreateToggle(DefaultResources());
        go.name = name;
        Place(go, parent, position, size);

        Toggle toggle = go.GetComponent<Toggle>();
        toggle.colors = ButtonColors();

        Text text = go.transform.Find("Label").GetComponent<Text>();
        text.font = AssetDatabase.LoadAssetAtPath<Font>(FontRegular);
        text.fontSize = SmallText;
        text.color = Palette.steenH;
        text.text = label;
        text.alignment = TextAnchor.MiddleLeft;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        // Groter vinkje, passend bij de tekst.
        var background = (RectTransform)go.transform.Find("Background");
        background.sizeDelta = new Vector2(40f, 40f);
        background.anchoredPosition = new Vector2(20f, -20f);
        var checkmark = (RectTransform)background.Find("Checkmark");
        checkmark.sizeDelta = new Vector2(36f, 36f);
        checkmark.GetComponent<Image>().color = Palette.nachtS;
        var labelRect = (RectTransform)text.transform;
        labelRect.offsetMin = new Vector2(64f, labelRect.offsetMin.y);
        return toggle;
    }

    public static ColorBlock ButtonColors()
    {
        ColorBlock colors = ColorBlock.defaultColorBlock;
        colors.normalColor = Palette.steen;
        colors.highlightedColor = Palette.goud;
        colors.selectedColor = Palette.goud;
        colors.pressedColor = Palette.goudD;
        colors.disabledColor = new Color(Palette.rots.r, Palette.rots.g, Palette.rots.b, 0.6f);
        return colors;
    }

    private static GameObject CreateRect(Transform parent, string name, Vector2 position, Vector2 size)
    {
        var go = new GameObject(name, typeof(RectTransform));
        Place(go, parent, position, size);
        return go;
    }

    private static void Place(GameObject go, Transform parent, Vector2 position, Vector2 size)
    {
        go.transform.SetParent(parent, false);
        var rect = (RectTransform)go.transform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static DefaultControls.Resources DefaultResources()
    {
        return new DefaultControls.Resources
        {
            standard = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd"),
            background = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd"),
            knob = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd"),
            checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd"),
        };
    }
}
