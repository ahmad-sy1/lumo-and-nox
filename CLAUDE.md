# Lumo & Nox

2D co-op puzzel-platformer voor twee spelers op één toetsenbord. Schoolproject voor Hackaton 3 (GameJam), thema: Unwanted powers. Gemaakt door Osama en Ahmad.

Het volledige ontwerp staat in `docs/design.md`. Lees dat eerst als je iets over het spel moet weten.

## Stack

- Unity (Universal 2D), versie: zie `ProjectSettings/ProjectVersion.txt`
- C# scripts in `Assets/Scripts/`
- Graphics zelf gemaakt (`Assets/Art/`, generator in `tools/generate_tiles.py`), geluid van Kenney.nl

## Structuur

- `Assets/Scripts/` - C# scripts, één klasse per bestand
- `Assets/Scenes/` - `Level1.unity`, `Level2.unity`
- `Assets/Prefabs/` - gedeelde objecten (spelers, lampen, deuren, hendels)
- `docs/` - ontwerp, klassendiagram, wireframes

## Regels

- Bewerk nooit `.unity`, `.prefab` of `.meta` bestanden met de hand. Dat doen wij in de Unity-editor.
- Raak `Library/`, `Temp/`, `Logs/` en `UserSettings/` niet aan.
- C#: PascalCase voor klassen en methodes, camelCase voor velden. Gebruik `[SerializeField] private` in plaats van `public` velden.
- Houd scripts klein en simpel. Dit is een MVP van 3 weken, geen grote architectuur.
- Als je een klasse toevoegt of verandert, werk dan ook `docs/class-diagram.md` bij.

## Documentatie

- Schrijf in het Nederlands, kort en simpel, in de wij-vorm.
- Korte zinnen. Alleen wat nodig is voor de opdracht.

## Git

- Werk per taak op een eigen branch (bijvoorbeeld `feature/swap-manager`).
- Commit-berichten in het Engels, kort.
- Push niet zonder het eerst te vragen.
- Zet Claude nergens neer: geen `Co-Authored-By`, geen "Generated with Claude Code" in commits, PR's of issues.