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
        // Nieuwe prefabs hier toevoegen.

        AssetDatabase.SaveAssets();
        Debug.Log("Lumo & Nox: prefabs gebouwd in " + PrefabsFolder);
    }

    [MenuItem("Lumo & Nox/Build Sandbox scene")]
    public static void BuildSandbox()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            return;
        }

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        CreateCamera();
        CreateGlobalLight();
        CreateBackground();
        CreateFloor();

        // Eén van elke prefab, naast elkaar op de vloer. Nieuwe prefabs hier toevoegen.
        PlacePrefab("Door", new Vector2(-9f, FloorTop + 1f));
        PlacePrefab("Lamp", new Vector2(-3f, FloorTop + 6.5f));
        PlacePrefab("MovingPlatform", new Vector2(6f, FloorTop + 3f));

        EditorSceneManager.SaveScene(scene, SandboxPath);
        Debug.Log("Lumo & Nox: Sandbox opgeslagen in " + SandboxPath);
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

    private static void PlacePrefab(string name, Vector2 position)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabsFolder + name + ".prefab");
        if (prefab == null)
        {
            BuildPrefabs();
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabsFolder + name + ".prefab");
        }

        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = position;
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

    private static Transform CreateMarker(GameObject parent, string name, Vector2 position)
    {
        var marker = new GameObject(name);
        marker.transform.SetParent(parent.transform, false);
        marker.transform.localPosition = position;
        return marker.transform;
    }
}
