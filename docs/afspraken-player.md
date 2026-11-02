# Afspraken: Player en SwapManager

Ahmad bouwt de puzzel-onderdelen, zones en de LevelManager (#16–#21). Osama bouwt `Player`, `Shape`-gedrag en `SwapManager` (#11–#13). Hier staat wat de code van Ahmad van die klassen verwacht. Zo kunnen we los van elkaar bouwen.

Ahmads code kent `Player` en `SwapManager` niet direct. Hij gebruikt twee interfaces. Osama's klassen hoeven die alleen te implementeren.

## Player

```csharp
public class Player : MonoBehaviour, IPlayer
```

| Wat | Waarom | Vanaf |
|---|---|---|
| Tag `Player` op het GameObject | `PressButton`, `Checkpoint`, `Crystal`, `ExitPortal` en `MovingPlatform` herkennen een speler aan de tag. | #16, #17 |
| Een `Rigidbody2D` en een niet-trigger `Collider2D` | Anders werken triggers en het meebewegen op `MovingPlatform` niet. | #16 |
| `Interact()` zoekt een `Lever` binnen bereik en roept `Lever.Use()` aan | De `Lever` heeft een trigger-collider als bereik. Bijvoorbeeld met `Physics2D.OverlapCircleAll` rond de speler. | #17 |
| `Shape CurrentShape { get; }` (uit `IPlayer`) | Zones kijken of de vorm gevaarlijk is. | #18 |
| `void Die()` (uit `IPlayer`) | Een zone roept dit aan als de speler doodgaat. `Die()` speelt het effect af en roept `LevelManager.OnPlayerDied()` aan. Het mag vaker achter elkaar aangeroepen worden. | #18, #20 |
| `void Respawn(Vector2 position)` (uit `IPlayer`) | De `LevelManager` zet beide spelers terug. `Respawn` zet de positie, zet de snelheid op 0 en zet de startvorm terug (Lumo licht, Nox schaduw). | #20 |
| `SetShape(Shape newShape)` | Voor de `SwapManager` bij de wissel. Ahmads code roept dit niet aan. | #12 |
| Gloed: `LightPlatform.SetRevealed(bool)` aanroepen voor platforms in de buurt | Lichtplatforms zijn alleen zichtbaar en betreedbaar met Gloed. | #19 |
| Schaduwstap: `ShadowWall.SetPassable(collider, bool)` aanroepen met de eigen collider | Alleen deze speler mag door de muur. Bij loslaten de speler uit de muur duwen als hij er nog in staat. | #19 |
| Nox: `PushBox.Push(richting)` aanroepen als hij tegen een kist loopt | Alleen Nox (`canPushBoxes`) kan duwen. Lumo duwt de kist niet weg. | #19 |

De `LevelManager` vind je met `FindAnyObjectByType<LevelManager>()`.

## SwapManager

```csharp
public class SwapManager : MonoBehaviour, ISwapTimer
```

| Wat | Waarom | Vanaf |
|---|---|---|
| `void ResetTimer()` (uit `ISwapTimer`) | De `LevelManager` start de wissel-timer opnieuw bij een checkpoint en na een respawn. | #20 |

Als het level klaar is, zet de `LevelManager` `Time.timeScale` op 0. Gebruik in de `SwapManager` dus `Time.deltaTime`, dan stopt de timer vanzelf.

## HUD

De HUD (#14) kan de stand lezen uit de `LevelManager`: `Crystals` (0–3), `Deaths` en `ElapsedTime` (seconden).

## Interfaces

```csharp
public enum Shape { Light, Shadow }

public interface IPlayer
{
    Shape CurrentShape { get; }
    void Die();
    void Respawn(Vector2 position);
}

public interface ISwapTimer
{
    void ResetTimer();
}
```

## Testen zonder Player

In de Sandbox staan twee `PlayerDummy`-objecten (Lumo en Nox). Ze implementeren `IPlayer` en hebben de tag `Player`, maar ze kunnen niet lopen. Tijdens Play kun je ze in de Scene-view in een zone, op een knop of in het portaal slepen. Zodra de echte `Player` er is, kunnen de dummies weg.
