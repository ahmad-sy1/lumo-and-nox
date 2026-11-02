using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Menu "Lumo & Nox": maakt prefabs en de Sandbox-scene vanuit code, zodat we .prefab en .unity
/// nooit met de hand bewerken. Elke nieuwe issue voegt hier zijn eigen prefab toe en zet hem in de Sandbox.
/// </summary>
public static class LumoTools
{
    private const string TilesFolder = "Assets/Art/Tiles/";
    private const string PrefabsFolder = "Assets/Prefabs/";
    private const string SandboxPath = "Assets/Scenes/Sandbox.unity";
    private const string SquareSpritePath =
        "Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png";

    // Zichtbaar deel van de Sandbox: 30 x 17 tegels (16:9).
    private const int ViewWidth = 30;
    private const int ViewHeight = 17;
    private const float FloorTop = -ViewHeight / 2f + 2f;

    [MenuItem("Lumo & Nox/Build prefabs")]
    public static void BuildPrefabs()
    {
        BuildDoor();
        BuildLamp();
        BuildMovingPlatform();
        BuildLever();
        BuildPressButton();
        BuildZone<HazardZone>("HazardZone", "stekels", 2);
        BuildZone<DarkZone>("DarkZone", "donkere_poel", 2);
        BuildPlayerDummy();
        BuildPlayer("Lumo", "lumo_schaduw_sheet", Player.Controls.Wasd, Shape.Light, 14f, false);
        BuildPlayer("Nox", "nox_schaduw_sheet", Player.Controls.Arrows, Shape.Shadow, 12f, true);
        BuildLightPlatform();
        BuildShadowWall();
        BuildPushBox();
        BuildCheckpoint();
        BuildLevelManager();
        BuildCrystal();
        BuildExitPortal();
        BuildGameManager();
        // Nieuwe prefabs hier toevoegen.

        AssetDatabase.SaveAssets();
        Debug.Log("Lumo & Nox: prefabs gebouwd in " + PrefabsFolder);
    }

    /// <summary>
    /// Bouwt eerst alle prefabs opnieuw en daarna de Sandbox.
    /// </summary>
    [MenuItem("Lumo & Nox/Build Sandbox scene")]
    public static void BuildSandbox()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        BuildPrefabs();
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCamera();
        CreateGlobalLight();
        CreateBackground();
        CreateFloor();
        PlaceTestSetup();

        EditorSceneManager.SaveScene(scene, SandboxPath);
        Debug.Log("Lumo & Nox: Sandbox opgeslagen in " + SandboxPath);
    }

    /// <summary>
    /// Eén opstelling per issue, zodat elk acceptatiecriterium te testen is. Nieuwe opstellingen hier toevoegen.
    /// </summary>
    private static void PlaceTestSetup()
    {
        // #16 en #17: hendel -> deur, drukknop -> deur.
        GameObject leverDoor = PlacePrefab("Door", new Vector2(-11.5f, FloorTop + 1f), "Door (hendel)");
        GameObject lever = PlacePrefab("Lever", new Vector2(-10f, FloorTop + 0.5f));
        SetField(lever.GetComponent<Lever>(), "target", leverDoor.GetComponent<Door>());

        GameObject buttonDoor = PlacePrefab("Door", new Vector2(-8f, FloorTop + 1f), "Door (drukknop)");
        GameObject button = PlacePrefab("PressButton", new Vector2(-6.5f, FloorTop + 0.5f));
        SetField(button.GetComponent<PressButton>(), "target", buttonDoor.GetComponent<Door>());

        // #19: kist tussen drukknop en lamp. Naar links = op de knop, naar rechts = onder de straal.
        PlacePrefab("PushBox", new Vector2(-5f, FloorTop + 0.5f));

        // #16 en #18: lamp met LightZone als straal, en bewegend platform.
        PlacePrefab("Lamp", new Vector2(-3f, FloorTop + 6.5f));
        PlacePrefab("MovingPlatform", new Vector2(8f, FloorTop + 5f));

        // #18: zones. #19: lichtplatform boven de poel en een schaduwmuur.
        PlacePrefab("DarkZone", new Vector2(2f, FloorTop + 0.5f));
        PlacePrefab("LightPlatform", new Vector2(2f, FloorTop + 2.5f));
        PlacePrefab("HazardZone", new Vector2(5f, FloorTop + 0.5f));
        PlacePrefab("ShadowWall", new Vector2(11f, FloorTop + 1.5f));

        // #20: LevelManager met startpunt, en een checkpoint tussen lamp en poel.
        var start = new GameObject("Start");
        start.transform.position = new Vector2(-13.5f, FloorTop + 0.75f);
        GameObject levelManager = PlacePrefab("LevelManager", Vector2.zero);
        SetField(levelManager.GetComponent<LevelManager>(), "startPoint", start.transform);
        PlacePrefab("Checkpoint", new Vector2(-1f, FloorTop + 1f));

        // #21: drie kristallen (boven de lantaarn, boven het lichtplatform, boven het bewegende platform)
        // en het uitgangsportaal rechts.
        PlacePrefab("Crystal", new Vector2(-1f, FloorTop + 3.5f), "Crystal 1");
        PlacePrefab("Crystal", new Vector2(2f, FloorTop + 4f), "Crystal 2");
        PlacePrefab("Crystal", new Vector2(8f, FloorTop + 7f), "Crystal 3");
        PlacePrefab("ExitPortal", new Vector2(13.5f, FloorTop + 1f));

        // #22: GameManager (blijft bestaan tussen scenes, beheert save.json).
        PlacePrefab("GameManager", Vector2.zero);

        // #11: de echte spelers. De PlayerDummy-prefab blijft bestaan voor losse tests.
        PlacePrefab("Lumo", new Vector2(-14f, FloorTop + 1f));
        PlacePrefab("Nox", new Vector2(-13f, FloorTop + 1f));
    }

    // ---------- Prefabs ----------

    private static void BuildDoor()
    {
        var door = new GameObject("Door");
        AddSprite(door, "deur_dicht");
        door.AddComponent<Door>();

        SetField(door.GetComponent<Door>(), "closedSprite", LoadTile("deur_dicht"));
        SetField(door.GetComponent<Door>(), "openSprite", LoadTile("deur_open"));

        SavePrefab(door, "Door");
    }

    private static void BuildLamp()
    {
        const float beamLength = 6f;

        var lamp = new GameObject("Lamp");
        AddSprite(lamp, "lamp_aan").sortingOrder = 1;

        // Lichtstraal naar beneden. Hier komt later de LightZone op.
        var beam = new GameObject("Beam");
        beam.transform.SetParent(lamp.transform, false);
        beam.transform.localPosition = new Vector3(0f, -0.5f - beamLength / 2f, 0f);
        beam.transform.localScale = new Vector3(1f, beamLength, 1f);
        AddSprite(beam, "lichtstraal");
        beam.AddComponent<LightZone>();

        Lamp script = lamp.AddComponent<Lamp>();
        SetField(script, "beam", beam);
        SetField(script, "onSprite", LoadTile("lamp_aan"));
        SetField(script, "offSprite", LoadTile("lamp_uit"));

        SavePrefab(lamp, "Lamp");
    }

    private static void BuildMovingPlatform()
    {
        const int width = 3;
        const float range = 3f;

        // De punten staan naast het platform, niet eronder: anders bewegen ze mee.
        var root = new GameObject("MovingPlatform");

        var platform = new GameObject("Platform");
        platform.transform.SetParent(root.transform, false);
        for (int i = 0; i < width; i++)
        {
            var tile = new GameObject("Tile" + i);
            tile.transform.SetParent(platform.transform, false);
            tile.transform.localPosition = new Vector3(i - (width - 1) / 2f, 0f, 0f);
            AddSprite(tile, "licht_platform");
        }

        var body = platform.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.interpolation = RigidbodyInterpolation2D.Interpolate;

        var collider = platform.AddComponent<BoxCollider2D>();
        Bounds bounds = LoadTile("licht_platform").bounds;
        collider.size = new Vector2(width, bounds.size.y);
        collider.offset = new Vector2(0f, bounds.center.y);

        Transform pointA = CreateMarker(root, "PointA", new Vector2(-range, 0f));
        Transform pointB = CreateMarker(root, "PointB", new Vector2(range, 0f));

        MovingPlatform script = platform.AddComponent<MovingPlatform>();
        SetField(script, "pointA", pointA);
        SetField(script, "pointB", pointB);

        SavePrefab(root, "MovingPlatform");
    }

    private static void BuildLever()
    {
        var lever = new GameObject("Lever");
        AddSprite(lever, "hendel_links");

        // De trigger is het bereik waarin de Player de hendel kan gebruiken.
        Lever script = lever.AddComponent<Lever>();
        lever.GetComponent<BoxCollider2D>().size = new Vector2(1.5f, 1f);
        SetField(script, "onSprite", LoadTile("hendel_rechts"));
        SetField(script, "offSprite", LoadTile("hendel_links"));

        SavePrefab(lever, "Lever");
    }

    private static void BuildPressButton()
    {
        var button = new GameObject("PressButton");
        AddSprite(button, "drukknop_omhoog");

        PressButton script = button.AddComponent<PressButton>();
        SetField(script, "upSprite", LoadTile("drukknop_omhoog"));
        SetField(script, "downSprite", LoadTile("drukknop_ingedrukt"));

        SavePrefab(button, "PressButton");
    }

    private static void BuildZone<T>(string name, string tile, int width) where T : Zone
    {
        GameObject zone = CreateTileBlock(name, tile, width);
        zone.GetComponent<BoxCollider2D>().isTrigger = true;
        zone.AddComponent<T>();
        SavePrefab(zone, name);
    }

    private static void BuildLightPlatform()
    {
        GameObject platform = CreateTileBlock("LightPlatform", "lichtplatform_verborgen", 2);
        Bounds bounds = LoadTile("lichtplatform_zichtbaar").bounds;
        var collider = platform.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(2f, bounds.size.y);
        collider.offset = new Vector2(0f, bounds.center.y);

        LightPlatform script = platform.AddComponent<LightPlatform>();
        SetField(script, "revealedSprite", LoadTile("lichtplatform_zichtbaar"));
        SetField(script, "hiddenSprite", LoadTile("lichtplatform_verborgen"));

        SavePrefab(platform, "LightPlatform");
    }

    private static void BuildShadowWall()
    {
        GameObject wall = CreateTileBlock("ShadowWall", "schaduwmuur", 1, 3);
        wall.AddComponent<ShadowWall>();
        SavePrefab(wall, "ShadowWall");
    }

    private static void BuildPushBox()
    {
        var box = new GameObject("PushBox");
        AddSprite(box, "kist").sortingOrder = 2;
        box.AddComponent<PushBox>();
        box.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        SavePrefab(box, "PushBox");
    }

    private static void BuildCheckpoint()
    {
        var checkpoint = new GameObject("Checkpoint");
        AddSprite(checkpoint, "lantaarn_uit");
        Checkpoint script = checkpoint.AddComponent<Checkpoint>();
        SetField(script, "litSprite", LoadTile("lantaarn_aan"));
        SavePrefab(checkpoint, "Checkpoint");
    }

    private static void BuildCrystal()
    {
        var crystal = new GameObject("Crystal");
        AddSprite(crystal, "kristal").sortingOrder = 3;
        crystal.AddComponent<Crystal>();
        SavePrefab(crystal, "Crystal");
    }

    private static void BuildExitPortal()
    {
        var portal = new GameObject("ExitPortal");
        SpriteRenderer renderer = AddSprite(portal, "portaal");
        renderer.color = new Color(0.6f, 0.6f, 0.6f);
        portal.AddComponent<ExitPortal>();
        SavePrefab(portal, "ExitPortal");
    }

    private static void BuildGameManager()
    {
        var gameManager = new GameObject("GameManager");
        gameManager.AddComponent<GameManager>();
        SavePrefab(gameManager, "GameManager");
    }

    private static void BuildLevelManager()
    {
        var levelManager = new GameObject("LevelManager");
        levelManager.AddComponent<LevelManager>();
        SavePrefab(levelManager, "LevelManager");
    }

    /// <summary>
    /// Blok van width x height tegels als children, met één BoxCollider2D op de root.
    /// </summary>
    private static GameObject CreateTileBlock(string name, string tile, int width, int height = 1)
    {
        var root = new GameObject(name);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var part = new GameObject($"Tile{x}_{y}");
                part.transform.SetParent(root.transform, false);
                part.transform.localPosition = new Vector3(x - (width - 1) / 2f, y - (height - 1) / 2f, 0f);
                AddSprite(part, tile);
            }
        }

        // Eén rij: collider zo hoog als het plaatje. Meer rijen: hele tegels.
        Bounds bounds = LoadTile(tile).bounds;
        var collider = root.AddComponent<BoxCollider2D>();
        collider.size = height == 1 ? new Vector2(width, bounds.size.y) : new Vector2(width, height);
        collider.offset = height == 1 ? new Vector2(0f, bounds.center.y) : Vector2.zero;
        return root;
    }

    private static void BuildPlayerDummy()
    {
        var dummy = new GameObject("PlayerDummy");
        dummy.tag = "Player";
        dummy.transform.localScale = new Vector3(0.8f, 1.5f, 1f);

        SpriteRenderer renderer = dummy.AddComponent<SpriteRenderer>();
        renderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(SquareSpritePath);
        renderer.sortingOrder = 5;

        dummy.AddComponent<PlayerDummy>();
        dummy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        SavePrefab(dummy, "PlayerDummy");
    }

    private static void BuildPlayer(string name, string sheet, Player.Controls controls, Shape startShape,
        float jumpForce, bool canPushBoxes)
    {
        var player = new GameObject(name);
        player.tag = "Player";

        SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
        renderer.sprite = LoadCharacterSprite(sheet);
        renderer.sortingOrder = 5;

        var body = player.AddComponent<Rigidbody2D>();
        body.gravityScale = 3f;

        // Collider iets kleiner dan het plaatje, zodat je niet blijft haken aan randen.
        var collider = player.AddComponent<BoxCollider2D>();
        Bounds bounds = renderer.sprite.bounds;
        collider.size = new Vector2(bounds.size.x * 0.7f, bounds.size.y * 0.9f);
        collider.offset = new Vector2(bounds.center.x, bounds.min.y + collider.size.y / 2f);

        Player script = player.AddComponent<Player>();
        SetEnum(script, "controls", (int)controls);
        SetEnum(script, "startShape", (int)startShape);
        SetFloat(script, "jumpForce", jumpForce);
        SetBool(script, "canPushBoxes", canPushBoxes);

        SavePrefab(player, name);
    }

    // ---------- Sandbox ----------

    private static void CreateCamera()
    {
        var go = new GameObject("Main Camera");
        go.tag = "MainCamera";
        go.transform.position = new Vector3(0f, 0f, -10f);

        Camera camera = go.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = ViewHeight / 2f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Palette.nachtS;
        go.AddComponent<UniversalAdditionalCameraData>();
    }

    private static void CreateGlobalLight()
    {
        var go = new GameObject("Global Light 2D");
        Light2D light = go.AddComponent<Light2D>();
        light.lightType = Light2D.LightType.Global;
    }

    private static void CreateBackground()
    {
        var square = AssetDatabase.LoadAssetAtPath<Sprite>(SquareSpritePath);
        if (square == null)
        {
            Debug.LogWarning("Lumo & Nox: Square-sprite niet gevonden, alleen de camerakleur als achtergrond.");
            return;
        }

        var parent = new GameObject("Achtergrond");
        CreateBackgroundHalf(parent, "Licht", square, Palette.nachtL, -ViewWidth / 4f);
        CreateBackgroundHalf(parent, "Schaduw", square, Palette.nachtS, ViewWidth / 4f);
    }

    private static void CreateBackgroundHalf(GameObject parent, string name, Sprite square, Color color, float x)
    {
        var half = new GameObject(name);
        half.transform.SetParent(parent.transform, false);
        half.transform.localPosition = new Vector3(x, 0f, 0f);
        half.transform.localScale = new Vector3(ViewWidth / 2f, ViewHeight + 1f, 1f);

        SpriteRenderer renderer = half.AddComponent<SpriteRenderer>();
        renderer.sprite = square;
        renderer.color = color;
        renderer.sortingOrder = -10;
    }

    private static void CreateFloor()
    {
        var floor = new GameObject("Vloer");

        for (int i = 0; i < ViewWidth; i++)
        {
            bool light = i < ViewWidth / 2;
            float x = i - ViewWidth / 2f + 0.5f;
            CreateTile(floor, light ? "licht_vloer_top" : "schaduw_vloer_top", new Vector2(x, FloorTop - 0.5f));
            CreateTile(floor, light ? "licht_vloer_vul" : "schaduw_vloer_vul", new Vector2(x, FloorTop - 1.5f));
        }

        var collider = floor.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(ViewWidth, 2f);
        collider.offset = new Vector2(0f, FloorTop - 1f);
    }

    private static void CreateTile(GameObject parent, string tile, Vector2 position)
    {
        var go = new GameObject(tile);
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = position;
        AddSprite(go, tile);
    }

    private static GameObject PlacePrefab(string name, Vector2 position, string instanceName = null)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabsFolder + name + ".prefab");
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = position;
        if (instanceName != null)
        {
            instance.name = instanceName;
        }
        return instance;
    }

    // ---------- Helpers ----------

    /// <summary>
    /// Slaat een GameObject op als Assets/Prefabs/&lt;name&gt;.prefab (maakt of overschrijft) en ruimt het daarna op.
    /// </summary>
    public static GameObject SavePrefab(GameObject root, string name)
    {
        Directory.CreateDirectory(PrefabsFolder);
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabsFolder + name + ".prefab");
        Object.DestroyImmediate(root);
        return prefab;
    }

    public static Sprite LoadTile(string name)
    {
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(TilesFolder + name + ".png");
        if (sprite == null)
        {
            Debug.LogError("Lumo & Nox: tegel niet gevonden: " + name);
        }
        return sprite;
    }

    public static SpriteRenderer AddSprite(GameObject go, string tile)
    {
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = LoadTile(tile);
        return renderer;
    }

    /// <summary>
    /// Vult een [SerializeField] private veld, zoals in de Inspector.
    /// </summary>
    public static void SetField(Object target, string field, Object value)
    {
        var serialized = new SerializedObject(target);
        serialized.FindProperty(field).objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetEnum(Object target, string field, int value)
    {
        var serialized = new SerializedObject(target);
        serialized.FindProperty(field).enumValueIndex = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetFloat(Object target, string field, float value)
    {
        var serialized = new SerializedObject(target);
        serialized.FindProperty(field).floatValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void SetBool(Object target, string field, bool value)
    {
        var serialized = new SerializedObject(target);
        serialized.FindProperty(field).boolValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    /// <summary>
    /// Eerste plaatje (_0) van een karakter-sheet in Assets/Sprites/Characters/.
    /// </summary>
    public static Sprite LoadCharacterSprite(string sheet)
    {
        foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/Characters/" + sheet + ".png"))
        {
            if (asset is Sprite sprite && sprite.name == sheet + "_0")
            {
                return sprite;
            }
        }

        Debug.LogError("Lumo & Nox: karakter-sheet niet gevonden: " + sheet);
        return null;
    }

    private static Transform CreateMarker(GameObject parent, string name, Vector2 position)
    {
        var marker = new GameObject(name);
        marker.transform.SetParent(parent.transform, false);
        marker.transform.localPosition = position;
        return marker.transform;
    }
}
