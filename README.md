# Lumo & Nox

Een 2D co-op puzzel-platformer voor twee spelers op één toetsenbord. Lumo is een lichtwezentje en Nox is een schaduwwezen. Om de zoveel seconden wisselen ze van vorm. Een plek die net veilig was, is dan ineens gevaarlijk. Je moet dus samen plannen.

Schoolproject voor Hackaton 3 (GameJam), thema: **Unwanted powers**. Gemaakt door Osama en Ahmad.

## Het spel installeren en starten

We gebruiken Unity (Universal 2D). De versie staat in `ProjectSettings/ProjectVersion.txt` (nu 6000.6.7f1).

1. Installeer Unity Hub en daarin de Unity-versie hierboven.
2. Clone de repository: `git clone https://github.com/ahmad-sy1/lumo-and-nox.git`
3. Kies in Unity Hub **Add** → **Add project from disk** en selecteer de map `lumo-and-nox`.
4. Open het project. De eerste keer importeert Unity alle bestanden. Dat duurt even.
5. Open `Assets/Scenes/MainMenu.unity` en druk op **Play**.

### Een build maken

1. Kies **File** → **Build Profiles** (of **Build Settings**).
2. Controleer dat `MainMenu`, `Level1` en `Level2` in de lijst staan, in die volgorde.
3. Kies je platform (bijvoorbeeld macOS of Windows) en klik op **Build**.
4. Start het spel uit de map die je hebt gekozen.

### De levels opnieuw bouwen

Scenes en prefabs bewerken we niet met de hand. We maken ze met het menu **Lumo & Nox** in de Unity-editor:

- **Build prefabs**: alle gedeelde objecten (spelers, lampen, deuren, hendels, HUD).
- **Build Sandbox scene**: de testscene `Assets/Scenes/Sandbox.unity`.
- **Build Level 1** en **Build Level 2**: de twee echte levels.
- **Build all**: alles in één keer.

## Doel van het spel

Een level is klaar als **beide spelers tegelijk in het uitgangsportaal** staan.

- Elk level past op één scherm.
- In elk level liggen 3 kristallen. Die zijn optioneel en tellen mee voor je score.
- Op het winscherm zie je je tijd, je kristallen en hoe vaak je dood ging.
- Gaat één speler dood, dan gaan beiden terug naar het laatste checkpoint.

## De wissel

- Na elke ronde wisselen Lumo en Nox van vorm: licht wordt schaduw en schaduw wordt licht.
- In Level 1 duurt een ronde 30 seconden, in Level 2 20 seconden. De balk boven in beeld laat zien hoeveel tijd er nog is.
- Drie seconden voor de wissel klinkt een piep en wordt de balk rood. Het scherm knippert dan rustig. Dat kun je uitzetten in Opties.

## Besturing

Twee spelers op één toetsenbord.

| Actie | Speler 1 (Lumo) | Speler 2 (Nox) |
| --- | --- | --- |
| Lopen | A / D | Pijltje links / rechts |
| Springen | W | Pijltje omhoog |
| Kracht gebruiken | S | Pijltje omlaag |
| Hendel of knop gebruiken | E | Rechter Shift |
| Pauze | Esc | Esc |

In de menu's gebruik je de muis, of de pijltjes met Enter.

## Vormen en krachten

| | Licht-vorm | Schaduw-vorm |
| --- | --- | --- |
| Kracht | **Gloed**: maakt verborgen lichtplatforms zichtbaar | **Schaduwstap**: loopt door schaduwmuren |
| Zwakte | Gaat dood in donkere poelen | Gaat dood in lichtstralen |

Daarnaast heeft elke speler één eigen kracht die altijd blijft: Lumo springt hoger en Nox kan kisten duwen.

## HUD

- Boven in het midden: de wissel-timer.
- Linksboven: de vorm van Lumo. Rechtsboven: de vorm van Nox (zon = licht, maan = schaduw).
- Rechtsonder: het aantal kristallen.

## Ontwerp en documentatie

Het volledige ontwerp staat in `docs/`:

- [Ontwerp](docs/design.md): pitch, personages, besturing, levels, HUD en techniek.
- [Klassendiagram en data-model](docs/class-diagram.md), met de diagrammen in `docs/diagrams/`.
- [Use-cases](docs/use-cases.md).
- [Wireframes](docs/wireframes/README.md) van de schermen.
- [Credits](docs/credits.md): fonts en geluid.

## Mappen

- `Assets/Scripts/`: C# scripts, één klasse per bestand.
- `Assets/Scenes/`: `MainMenu`, `Level1`, `Level2` en de testscene `Sandbox`.
- `Assets/Prefabs/`: gedeelde objecten.
- `Assets/Editor/`: de tools van het menu **Lumo & Nox**.
- `Assets/Art/`: onze eigen graphics. De generator staat in `tools/generate_tiles.py`.
