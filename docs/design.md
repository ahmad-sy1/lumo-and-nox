# Lumo & Nox – Ontwerp

13 oktober 2026

## Pitch

Lumo & Nox is een 2D co-op puzzel-platformer voor twee spelers op één toetsenbord. Lumo is een lichtwezentje en Nox is een schaduwwezen. Ze moeten samen door een donker bos en een vuurtoren komen.

De twist: om de zoveel seconden wisselen ze van vorm. Lumo wordt schaduw en Nox wordt licht. Een plek die net veilig was, is na de wissel ineens gevaarlijk. Je moet dus samen plannen waar je staat als de wissel komt.

Jam-thema: **Unwanted powers**.

## Thema en twist

We kiezen Unwanted powers omdat de spelers hun vorm niet zelf kiezen. De wissel overkomt ze, en dat is vaak precies wat ze niet willen.

Zo werkt de wissel:

- Na elke ronde wisselen Lumo en Nox van vorm (licht wordt schaduw, schaduw wordt licht).
- In level 1 duurt een ronde 30 seconden, in level 2 20 seconden. Zo wordt het langzaam moeilijker.
- 3 seconden voor de wissel knippert het scherm en hoor je een piep. Zo kun je nog op tijd een veilige plek zoeken.
- Na de wissel horen ook de krachten bij de nieuwe vorm.
- De timer loopt altijd door, ook als een speler stilstaat.

## Personages

De speler houdt zijn figuur, maar de vorm (licht of schaduw) wisselt. De kracht en de zwakte horen bij de vorm. Daarnaast heeft elke figuur één eigen kracht die altijd blijft.

|  | Lumo | Nox |
| --- | --- | --- |
| Wie | Klein lichtbolletje met oogjes | Klein schaduwfiguurtje met gloeiende ogen |
| Start-vorm | Licht | Schaduw |
| Kracht in licht-vorm | Gloed: maakt verborgen lichtplatforms zichtbaar en betreedbaar | Zelfde kracht |
| Kracht in schaduw-vorm | Schaduwstap: loopt door schaduwmuren heen | Zelfde kracht |
| Zwakte licht-vorm | Gaat dood in donkere poelen | Zelfde zwakte |
| Zwakte schaduw-vorm | Gaat dood in lichtstralen | Zelfde zwakte |
| Eigen kracht (altijd) | Springt hoger | Kan kisten duwen |

## Besturing

Twee spelers op één toetsenbord.

| Actie | Speler 1 (Lumo) | Speler 2 (Nox) |
| --- | --- | --- |
| Lopen | A / D | Pijltje links / rechts |
| Springen | W | Pijltje omhoog |
| Kracht gebruiken | S | Pijltje omlaag |
| Hendel / knop gebruiken | E | Rechter Shift |
| Pauze | Esc | Esc |

In de menu's gebruik je de muis of de pijltjes met Enter.

## Doel van een level

Een level is klaar als beide spelers tegelijk in het uitgangsportaal staan.

- Elk level past op één scherm, dus we hebben geen bewegende camera nodig.
- In elk level liggen 3 kristallen. Die zijn optioneel en tellen mee voor je score.
- Op het winscherm zie je je tijd, het aantal kristallen en hoe vaak je dood ging.

## Gevaren en puzzel-onderdelen

De puzzels draaien om licht en schaduw slim aan en uit zetten, op het goede moment voor de wissel.

| Onderdeel | Wat het doet | Gevaarlijk voor |
| --- | --- | --- |
| Lichtstraal (lamp) | Fel licht in een vaste baan | Schaduw-vorm |
| Donkere poel | Pikzwart gebied op de grond | Licht-vorm |
| Stekels / afgrond | Gewoon gevaar | Allebei |
| Hendel | Zet een lamp aan of uit | – |
| Drukknop | Opent een deur zolang iemand erop staat | – |
| Deur | Gaat open door een hendel of knop | – |
| Lichtplatform | Alleen zichtbaar met Gloed | – |
| Schaduwmuur | Alleen door te lopen met Schaduwstap | – |
| Bewegend platform | Gaat heen en weer of op en neer | – |
| Kist | Alleen Nox kan hem duwen. Blokkeert een lichtstraal en maakt zo schaduw | – |

## Failstate en checkpoints

Als één speler doodgaat, beginnen jullie allebei opnieuw bij het laatste checkpoint. Je doet het samen, dus je faalt ook samen.

- Er zijn geen levens. Je kunt oneindig opnieuw proberen.
- Elk level heeft 1 of 2 checkpoints (een lantaarn die gaat branden als jullie er allebei langs lopen).
- Bij een checkpoint begint de wissel-timer opnieuw.
- De dood wordt geteld en staat op het winscherm.

## HUD

De HUD is klein, want het hele level moet op één scherm zichtbaar blijven.

- **Boven in het midden:** de wissel-timer als balk die leegloopt, met de seconden erbij. De laatste 3 seconden wordt de balk rood en knippert hij.
- **Linksboven:** icoon van Lumo met zijn huidige vorm (zon = licht, maan = schaduw).
- **Rechtsboven:** hetzelfde voor Nox.
- **Rechtsonder:** aantal kristallen (bijvoorbeeld 1/3).
- In het pauzemenu verdwijnt de HUD en wordt het spel wazig, net als in het Billy Sneak-voorbeeld.

## Menu's en navigatie

Het hoofdmenu is het centrum: van daaruit ga je naar spelen, level select, opties of afsluiten.

```mermaid
flowchart TD
    Start[Startscherm] --> Hoofd[Hoofdmenu]
    Hoofd <--> Opties[Opties]
    Hoofd --> Afsluiten[Afsluiten]
    Hoofd --> Select[Level select]
    Hoofd -- Spelen --> Level[Level spelen<br/>dood: terug naar checkpoint]
    Select --> Level
    Level -- Esc --> Pauze[Pauzemenu<br/>Opnieuw · Opties · Hoofdmenu]
    Pauze -- Verder --> Level
    Level -- Level klaar --> Win[Winscherm<br/>Volgend level · Opnieuw · Hoofdmenu]
```

De wireframes van alle schermen staan in [docs/wireframes](wireframes/README.md).

Met Esc open je tijdens het spelen het pauzemenu. Tijdens de pauze staat de wissel-timer stil. Opties is ook vanuit het pauzemenu te openen en gaat dan terug naar het pauzemenu.

In Opties kun je:

- het volume van muziek en geluidseffecten instellen;
- volledig scherm aan- of uitzetten;
- het knipperen bij de wissel op **Rustig** of **Uit** zetten;
- de besturing bekijken (niet aanpassen).

Instellingen worden meteen lokaal opgeslagen, samen met de voortgang (zie het data-model in [class-diagram.md](class-diagram.md)).

## Levels

We maken 2 levels. Elk level wordt door één persoon gebouwd.

**Level 1 – Het Nachtbos (tutorial)**

- Wissel elke 30 seconden.
- Leert de basis via het level zelf: eerst lopen en springen, dan een lichtstraal, dan een donkere poel.
- De eerste wissel gebeurt op een veilige plek, zodat de spelers rustig zien wat er gebeurt.
- Korte tekstbordjes in het level leggen de knoppen uit.

**Level 2 – De Vuurtoren**

- Wissel elke 20 seconden.
- Draaiende of bewegende lampen, kisten om schaduw te maken, en deuren die alleen open gaan als de ene speler op een knop blijft staan.
- Je moet hier echt samenwerken: de één houdt de deur open terwijl de ander erdoor gaat, voordat de wissel komt.

## Techniek, assets en klassen

We gebruiken **Unity** (Universal 2D) met **C#**. We kiezen Unity omdat daar veel meer hulp en tutorials voor te vinden zijn. Dat helpt ons als we vastlopen in de 3 weken.

- **Graphics:** zelf gemaakt in het palet van Lumo en Nox (licht = goed: warm en rond; schaduw = kwaad: koud en puntig), zie `docs/art/`. **Geluid:** Kenney (CC0).
- **Versiebeheer:** GitHub, met een main-branch en per taak een eigen branch. Mergen via een pull request die de ander bekijkt.

Belangrijkste klassen (basis voor het klassendiagram). Bijna alle klassen zijn een `MonoBehaviour` die we op een GameObject zetten. Elke klasse staat in een eigen C#-bestand in `Assets/Scripts/`.

| Klasse | Type | Taak |
| --- | --- | --- |
| Player | MonoBehaviour | Lopen, springen, vorm, kracht gebruiken, doodgaan |
| SwapManager | MonoBehaviour | Timer, waarschuwing, vorm van beide spelers wisselen |
| LightZone / DarkZone | MonoBehaviour (trigger-collider) | Checken of een speler in de verkeerde vorm erin staat |
| Lamp | MonoBehaviour | Lichtstraal aan/uit, eventueel bewegen |
| Lever / PressButton | MonoBehaviour | Iets aan- of uitzetten als er iemand gebruik van maakt |
| Door | MonoBehaviour | Open/dicht door een Lever of PressButton |
| MovingPlatform | MonoBehaviour | Heen en weer bewegen |
| PushBox | MonoBehaviour (met Rigidbody2D) | Alleen Nox kan hem duwen, blokkeert licht |
| Checkpoint | MonoBehaviour | Laatste veilige plek opslaan |
| Crystal | MonoBehaviour | Op te pakken, telt mee voor de score |
| LevelManager | MonoBehaviour | Respawn, deaths, tijd, level klaar |
| GameManager | MonoBehaviour (blijft bestaan tussen scenes) | Scenes laden, voortgang opslaan |
| HUD / Menu | MonoBehaviour (UI) | Schermen tonen |

Daarnaast is er een `OptionsMenu` voor het Opties-scherm. Het uitgewerkte klassendiagram en het data-model staan in [class-diagram.md](class-diagram.md).

## Taakverdeling en planning

We werken met 3 sprints van 1 week, met aan het eind van elke week een korte review en retrospective.

| Wie | Code | Level |
| --- | --- | --- |
| Osama | Player, SwapManager, HUD | Level 2 |
| Ahmad | Puzzel-onderdelen, menu's, LevelManager | Level 1 |

| Week | Wat |
| --- | --- |
| 1 – Ontwerpen | Dit document, klassendiagram, data-model, use case diagram, wireframes, activiteitendiagrammen, menu-flowchart, Git-repo, akkoord docent |
| 2 – Bouwen | Spelers en wissel werkend, puzzel-onderdelen, level 1 en 2 bouwen, menu's |
| 3 – Testen en presenteren | Laten testen, bugs fixen, documentatie, filmpje opnemen |

## Privacy, ethiek en veiligheid

Het spel verzamelt geen persoonlijke gegevens en werkt helemaal offline.

- Alleen de voortgang (welke levels gehaald zijn, beste tijd, kristallen) en de instellingen worden opgeslagen, in één bestand (`save.json`) op de eigen computer. Er gaat niets over internet.
- Geen geweld of bloed: als je doodgaat, valt je figuur uit elkaar in vonkjes of rook. Geschikt voor alle leeftijden.
- De wissel-waarschuwing is zowel te zien (knipperen en een rode timerbalk) als te horen (piep), zodat je hem niet mist. Het knipperen is standaard rustig, nooit fel.
- Voor mensen die gevoelig zijn voor flitsend licht kun je het knipperen in Opties helemaal uitzetten. De waarschuwing blijft dan werken via de piep en de rode timerbalk. Zo is het spel speelbaar voor iedereen, zonder dat iemand een waarschuwing mist.
- De graphics maken we zelf. Het geluid komt van Kenney.nl (CC0) en mag vrij gebruikt worden.

## Ontwerpdocumenten

Het ontwerp bestaat uit deze onderdelen. Samen dekken ze de data, de gebruiker en de logica van het spel.

| Laag | Schema | Waar |
| --- | --- | --- |
| Data | Klassendiagram en data-model (PlantUML) | [class-diagram.md](class-diagram.md) |
| Gebruiker | Wireframes van alle schermen + levelschets Level 1 | [wireframes](wireframes/README.md) |
| Gebruiker | Use case diagram (PlantUML) en beschrijvingen | [use-cases.md](use-cases.md) |
| Logica | Activiteitendiagrammen (PlantUML), 12 stuks | [activiteitendiagrammen](activiteitendiagrammen/) |
| Navigatie | Menu-flowchart | zie [Menu's en navigatie](#menus-en-navigatie) hierboven |
