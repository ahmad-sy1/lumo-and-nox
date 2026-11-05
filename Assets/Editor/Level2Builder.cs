using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Bouwt Level2.unity (De Vuurtoren): een deur met twee knoppen waar je samen doorheen moet, een bewegende
/// lamp, een donkere poel met lichtplatform, 3 kristallen, 2 checkpoints en het portaal. Wissel elke 20 s.
/// De prefabs komen uit LumoTools.
/// </summary>
public static class Level2Builder
{
    private const string ScenePath = "Assets/Scenes/Level2.unity";
    private const float FloorTop = LumoTools.FloorTop;
    private const float PoolLeft = 5f;
    private const float PoolRight = 11f;
    private const float DoorX = -8f;
    private const float RoundTime = 20f;

    [MenuItem("Lumo & Nox/Build Level 2")]
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
        Debug.Log("Lumo & Nox: Level 2 opgeslagen in " + ScenePath);
    }

    private static void PlaceLevel()
    {
        LumoTools.PlacePrefab("Lumo", new Vector2(-14f, FloorTop + 1f));
        LumoTools.PlacePrefab("Nox", new Vector2(-13f, FloorTop + 1f));

        // 1: de deur zit in een muur tot aan het plafond, dus je kunt er niet overheen springen.
        // De deur is 2 tegels hoog; de muur begint daarboven.
        GameObject wall = LumoTools.CreateTileBlock("Muur", "licht_platform", 1, 13);
        wall.transform.position = new Vector2(DoorX, FloorTop + 2f + 6.5f);
        GameObject door = LumoTools.PlacePrefab("Door", new Vector2(DoorX, FloorTop + 1f));

        // Twee knoppen sturen dezelfde deur aan: de een houdt hem open, de ander loopt erdoor en drukt
        // de tweede knop in. Dan mag de eerste loslaten en erdoor.
        PlaceButton("Knop links", new Vector2(-10.5f, FloorTop + 0.5f), door);
        PlaceButton("Knop rechts", new Vector2(-5.5f, FloorTop + 0.5f), door);
        Level1Builder.CreateSign("Bordje knoppen", "Twee knoppen\nSamen door de deur", new Vector2(-11.5f, FloorTop + 3f));
        LumoTools.PlacePrefab("Crystal", new Vector2(-6.5f, FloorTop + 1f), "Crystal 1");

        LumoTools.PlacePrefab("Checkpoint", new Vector2(-3f, FloorTop + 1f));

        // 2: een lamp die heen en weer zweeft. De straal is dodelijk voor de schaduw-vorm: timen maar.
        PlaceMovingLamp(new Vector2(-1f, FloorTop + 6.5f), new Vector2(3f, FloorTop + 6.5f));
        LumoTools.PlacePrefab("Crystal", new Vector2(1f, FloorTop + 1f), "Crystal 2");

        LumoTools.PlacePrefab("Checkpoint", new Vector2(4f, FloorTop + 1f), "Checkpoint 2");

        // 3: de poel is 6 breed, te ver om over te springen. De licht-vorm heeft Gloed nodig.
        // Het kristal hangt zo hoog dat je er alleen vanaf het lichtplatform bij kunt.
        LumoTools.PlacePrefab("DarkPool", new Vector2(6f, FloorTop), "DarkPool 1");
        LumoTools.PlacePrefab("DarkPool", new Vector2(8f, FloorTop), "DarkPool 2");
        LumoTools.PlacePrefab("DarkPool", new Vector2(10f, FloorTop), "DarkPool 3");
        LumoTools.PlacePrefab("LightPlatform", new Vector2(8f, FloorTop + 1.5f));
        LumoTools.PlacePrefab("Crystal", new Vector2(8f, FloorTop + 5.4f), "Crystal 3");

        LumoTools.PlacePrefab("ExitPortal", new Vector2(13.5f, FloorTop + 1f));
    }

    private static void PlaceButton(string name, Vector2 position, GameObject door)
    {
        GameObject button = LumoTools.PlacePrefab("PressButton", position, name);
        LumoTools.SetField(button.GetComponent<PressButton>(), "target", door.GetComponent<Door>());
    }

    // De Lamp-prefab krijgt hier een MovingPlatform erbij; de twee punten staan los van de lamp.
    private static void PlaceMovingLamp(Vector2 from, Vector2 to)
    {
        GameObject lamp = LumoTools.PlacePrefab("Lamp", from);
        var pointA = new GameObject("Lamp punt A");
        pointA.transform.position = from;
        var pointB = new GameObject("Lamp punt B");
        pointB.transform.position = to;

        lamp.AddComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        MovingPlatform mover = lamp.AddComponent<MovingPlatform>();
        LumoTools.SetField(mover, "pointA", pointA.transform);
        LumoTools.SetField(mover, "pointB", pointB.transform);
        LumoTools.SetFloat(mover, "speed", 1.5f);
    }

    private static void PlaceManagers()
    {
        var start = new GameObject("Start");
        start.transform.position = new Vector2(-13.5f, FloorTop + 0.75f);
        GameObject levelManager = LumoTools.PlacePrefab("LevelManager", Vector2.zero);
        LumoTools.SetField(levelManager.GetComponent<LevelManager>(), "startPoint", start.transform);
        var serialized = new SerializedObject(levelManager.GetComponent<LevelManager>());
        serialized.FindProperty("levelIndex").intValue = 1;
        serialized.ApplyModifiedPropertiesWithoutUndo();

        GameObject swapManager = LumoTools.PlacePrefab("SwapManager", Vector2.zero);
        LumoTools.SetFloat(swapManager.GetComponent<SwapManager>(), "roundTime", RoundTime);

        LumoTools.PlacePrefab("GameManager", Vector2.zero);
        LumoTools.PlacePrefab("HUD", Vector2.zero);
        LumoTools.PlacePrefab("WinScreen", Vector2.zero);
        UiBuilder.CreateEventSystem();

        GameObject options = LumoTools.PlacePrefab("OptionsMenu", Vector2.zero);
        GameObject pause = LumoTools.PlacePrefab("PauseMenu", Vector2.zero);
        LumoTools.SetField(pause.GetComponent<PauseMenu>(), "optionsMenu", options.GetComponent<OptionsMenu>());
    }

    // Doorlopende vloer, links licht en rechts schaduw. Op de plek van de poel geen bovenste tegel.
    private static void CreateFloor()
    {
        var floor = new GameObject("Vloer");
        float left = -LumoTools.ViewWidth / 2f;
        float right = LumoTools.ViewWidth / 2f;

        for (float x = left + 0.5f; x < right; x++)
        {
            bool light = x < 0f;
            if (x < PoolLeft || x > PoolRight)
            {
                LumoTools.CreateTile(floor, light ? "licht_vloer_top" : "schaduw_vloer_top", new Vector2(x, FloorTop - 0.5f));
            }

            LumoTools.CreateTile(floor, light ? "licht_vloer_vul" : "schaduw_vloer_vul", new Vector2(x, FloorTop - 1.5f));
        }

        AddBox(floor, left, right, FloorTop - 2f, FloorTop);
    }

    // Onzichtbare muren aan de schermranden.
    private static void CreateBounds()
    {
        var bounds = new GameObject("Randen");
        float left = -LumoTools.ViewWidth / 2f;
        float right = LumoTools.ViewWidth / 2f;
        float bottom = FloorTop - 2f;

        AddBox(bounds, left - 1f, left, bottom, LumoTools.ViewHeight);
        AddBox(bounds, right, right + 1f, bottom, LumoTools.ViewHeight);
    }

    private static void AddBox(GameObject go, float left, float right, float bottom, float top)
    {
        var box = go.AddComponent<BoxCollider2D>();
        box.size = new Vector2(right - left, top - bottom);
        box.offset = new Vector2((left + right) / 2f, (bottom + top) / 2f);
    }
}
