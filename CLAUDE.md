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
- `.unity` en `.prefab` maken we niet met de hand, maar via `Assets/Editor/LumoTools.cs` (menu "Lumo & Nox"). Unity schrijft de bestanden dan zelf.
- Nieuwe graphics: pas `tools/generate_tiles.py` aan en draai het opnieuw.
- Testen doen we in `Assets/Scenes/Sandbox.unity`. Echte levels bouwen we pas in `Level1` en `Level2`.
- Raak `Library/`, `Temp/`, `Logs/` en `UserSettings/` niet aan.
- C#: PascalCase voor klassen en methodes, camelCase voor velden. Gebruik `[SerializeField] private` in plaats van `public` velden.
- Houd scripts klein en simpel. Dit is een MVP van 3 weken, geen grote architectuur.
- Als je een klasse toevoegt of verandert, werk dan ook `docs/class-diagram.md` bij.

## Documentatie

- Schrijf in het Nederlands, kort en simpel, in de wij-vorm.
- Korte zinnen. Alleen wat nodig is voor de opdracht.

## Git en Scrum-board

Wij werken met GitHub issues, milestones (Sprint 1, 2 en 3) en het project-board
"Lumo & Nox – Scrum" (eigenaar `ahmad-sy1`). Kolommen: Product Backlog → Sprint Backlog →
In Progress → In Review → Done. Elke taak is een issue.

### Workflow per issue (altijd in deze volgorde)

1. **Oppakken:** wijs het issue toe aan wie eraan werkt (`gh issue edit <nr> --add-assignee @me`)
   en zet het op het board op **In Progress**.
2. **Branch:** `feature/<nr>-<onderwerp>` (bijvoorbeeld `feature/17-lever-pressbutton`),
   vanaf een verse `main`, tenzij er bewust gestapeld wordt op een open PR.
3. **Bouwen:** volg de acceptatiecriteria van het issue en de gelinkte ontwerpbestanden.
   Compileer in batchmode en check dat er geen `error CS` is.
4. **PR maken:** push de branch en open een PR naar `main`:
   - titel = titel van het issue
   - beschrijving in het Nederlands: wat er gebouwd is, hoe je het test, en `Closes #<nr>`
   - zelfde milestone als het issue, reviewer = de ander (Osama of Ahmad)
   - bouwt de PR voort op een andere open PR: zet erin "Eerst #<andere PR> mergen".
5. **Op review zetten:** zet het issue op **In Review**. Voeg de PR zelf NIET toe aan het board;
   door `Closes #<nr>` is hij al gekoppeld en zie je hem als link in de issue-kaart.
6. **Nooit zelf mergen.** De ander reviewt en merget. Door `Closes #<nr>` sluit het issue
   en gaat de kaart naar **Done**.

Board-status zetten gaat met `gh project item-edit` (zoek project-nummer, veld-ID's en
optie-ID's eerst op met `gh project list --owner ahmad-sy1` en `gh project field-list`).

### Overige afspraken

- Pak geen issues op die aan iemand anders zijn toegewezen.
- Push en PR maken mag zonder te vragen zodra de build slaagt. Mergen en force-pushen nooit.
- Commit-berichten in het Engels, kort.
- Zet Claude nergens neer: geen `Co-Authored-By`, geen "Generated with Claude Code" in commits, PR's of issues.
