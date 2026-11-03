using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Bouwt Level1.unity (Het Nachtbos) volgens levelschets 08: start, gat met stekels, lamp en hendel,
/// checkpoint, donkere poel met lichtplatform en het portaal. De prefabs komen uit LumoTools.
/// </summary>
public static class Level1Builder
{
    private const string ScenePath = "Assets/Scenes/Level1.unity";
    private const float FloorTop = LumoTools.FloorTop;
    private const float PitLeft = -8f;
    private const float PitRight = -6f;
    private const float PitCenter = (PitLeft + PitRight) / 2f;

    // Lumo springt 3,3 tegels hoog en Nox 2,4. Op 2,9 tegels komt dus alleen Lumo.
    private const float HighPlatformSurface = 2.9f;

    [MenuItem("Lumo & Nox/Build Level 1")]
    public static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        LumoTools.CreateCamera();
        LumoTools.CreateGlobalLight();
        LumoTools.CreateBackground();
        CreateFloor();
        CreateBounds();
        PlaceLevel();
        PlaceManagers();

        EditorSceneManager.SaveScene(scene, ScenePath);
        Debug.Log("Lumo & Nox: Level 1 opgeslagen in " + ScenePath);
    }

    private static void PlaceLevel()
    {
        // 1 en 2: start en bordje met lopen en springen.
        LumoTools.PlacePrefab("Lumo", new Vector2(-14f, FloorTop + 1f));
        LumoTools.PlacePrefab("Nox", new Vector2(-13f, FloorTop + 1f));
        CreateSign("Bordje besturing", "Lumo: A D en W\nNox: pijltjes", new Vector2(-10f, FloorTop + 0.9f));

        // 3: gat met stekels en een hoog platform met kristal, alleen Lumo komt erbij.
        LumoTools.PlacePrefab("HazardZone", new Vector2(PitCenter, FloorTop - 1.5f));
        GameObject platform = LumoTools.CreateTileBlock("Hoog platform", "licht_platform", 3);
        platform.transform.position = new Vector2(-9.5f, FloorTop + HighPlatformSurface - 0.5f);
        LumoTools.PlacePrefab("Crystal", new Vector2(-9.5f, FloorTop + HighPlatformSurface + 0.6f), "Crystal 1");

        // 4: de straal is dodelijk voor Nox. Lumo zet de lamp uit met de hendel.
        // De hendel begint op aan: de eerste keer gebruiken zet de lamp dan uit.
        GameObject lamp = LumoTools.PlacePrefab("Lamp", new Vector2(-1.5f, FloorTop + 6.5f));
        GameObject lever = LumoTools.PlacePrefab("Lever", new Vector2(-4.5f, FloorTop + 0.5f));
        LumoTools.SetField(lever.GetComponent<Lever>(), "target", lamp.GetComponent<Lamp>());
        LumoTools.SetBool(lever.GetComponent<Lever>(), "isOn", true);
        CreateSign("Bordje hendel", "Hendel\nE of R-Shift", new Vector2(-4.5f, FloorTop + 2.6f));
        LumoTools.PlacePrefab("Crystal", new Vector2(-1.5f, FloorTop + 1f), "Crystal 2");

        // 5 en 6: checkpoint-lantaarn en bordje met de kracht.
        LumoTools.PlacePrefab("Checkpoint", new Vector2(1f, FloorTop + 1f));
        CreateSign("Bordje kracht", "Kracht\nS of pijl omlaag", new Vector2(3f, FloorTop + 2.6f));

        // 7: de poel is 6 breed, te ver om over te springen. De licht-vorm heeft dus Gloed nodig.
        // Het kristal hangt zo hoog dat je er alleen vanaf het lichtplatform bij kunt.
        LumoTools.PlacePrefab("DarkZone", new Vector2(5f, FloorTop + 0.5f), "DarkZone 1");
        LumoTools.PlacePrefab("DarkZone", new Vector2(7f, FloorTop + 0.5f), "DarkZone 2");
        LumoTools.PlacePrefab("DarkZone", new Vector2(9f, FloorTop + 0.5f), "DarkZone 3");
        LumoTools.PlacePrefab("LightPlatform", new Vector2(7f, FloorTop + 1.5f));
        LumoTools.PlacePrefab("Crystal", new Vector2(7f, FloorTop + 5.4f), "Crystal 3");

        // 8: portaal, beide spelers tegelijk erin.
        LumoTools.PlacePrefab("ExitPortal", new Vector2(13f, FloorTop + 1f));
    }

    private static void PlaceManagers()
    {
        var start = new GameObject("Start");
        start.transform.position = new Vector2(-13.5f, FloorTop + 0.75f);
        GameObject levelManager = LumoTools.PlacePrefab("LevelManager", Vector2.zero);
        LumoTools.SetField(levelManager.GetComponent<LevelManager>(), "startPoint", start.transform);

        LumoTools.PlacePrefab("GameManager", Vector2.zero);
        LumoTools.PlacePrefab("WinScreen", Vector2.zero);
        UiBuilder.CreateEventSystem();

        GameObject options = LumoTools.PlacePrefab("OptionsMenu", Vector2.zero);
        GameObject pause = LumoTools.PlacePrefab("PauseMenu", Vector2.zero);
        LumoTools.SetField(pause.GetComponent<PauseMenu>(), "optionsMenu", options.GetComponent<OptionsMenu>());
    }

    // Vloer van twee delen met het gat ertussen. Links licht, rechts schaduw, net als de Sandbox.
    private static void CreateFloor()
    {
        var floor = new GameObject("Vloer");
        float left = -LumoTools.ViewWidth / 2f;
        float right = LumoTools.ViewWidth / 2f;

        for (float x = left + 0.5f; x < right; x++)
        {
            if (x > PitLeft && x < PitRight)
            {
                continue;
            }

            bool light = x < 0f;
            LumoTools.CreateTile(floor, light ? "licht_vloer_top" : "schaduw_vloer_top", new Vector2(x, FloorTop - 0.5f));
            LumoTools.CreateTile(floor, light ? "licht_vloer_vul" : "schaduw_vloer_vul", new Vector2(x, FloorTop - 1.5f));
        }

        AddBox(floor, left, PitLeft, FloorTop - 2f, FloorTop);
        AddBox(floor, PitRight, right, FloorTop - 2f, FloorTop);
    }

    // Onzichtbare muren aan de schermranden en een bodem onder het gat.
    private static void CreateBounds()
    {
        var bounds = new GameObject("Randen");
        float left = -LumoTools.ViewWidth / 2f;
        float right = LumoTools.ViewWidth / 2f;
        float bottom = FloorTop - 2f;

        AddBox(bounds, left - 1f, left, bottom, LumoTools.ViewHeight);
        AddBox(bounds, right, right + 1f, bottom, LumoTools.ViewHeight);
        AddBox(bounds, PitLeft, PitRight, bottom - 1f, bottom);
    }

    private static void AddBox(GameObject go, float left, float right, float bottom, float top)
    {
        var box = go.AddComponent<BoxCollider2D>();
        box.size = new Vector2(right - left, top - bottom);
        box.offset = new Vector2((left + right) / 2f, (bottom + top) / 2f);
    }

    // Bordje zonder collider: spelers lopen er gewoon langs.
    private static void CreateSign(string name, string text, Vector2 position)
    {
        var sign = new GameObject(name);
        sign.transform.position = position;

        var board = new GameObject("Bord");
        board.transform.SetParent(sign.transform, false);
        board.transform.localScale = new Vector3(4.2f, 1.4f, 1f);
        SpriteRenderer boardRenderer = board.AddComponent<SpriteRenderer>();
        boardRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(LumoTools.SquareSpritePath);
        boardRenderer.color = Palette.rots;
        boardRenderer.sortingOrder = -2;

        var label = new GameObject("Tekst");
        label.transform.SetParent(sign.transform, false);
        Font font = AssetDatabase.LoadAssetAtPath<Font>(UiBuilder.FontRegular);
        TextMesh mesh = label.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.font = font;
        mesh.fontSize = 32;
        mesh.characterSize = 0.1f;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.color = Palette.steenH;

        MeshRenderer textRenderer = label.GetComponent<MeshRenderer>();
        textRenderer.sharedMaterial = font.material;
        textRenderer.sortingOrder = -1;
    }
}
