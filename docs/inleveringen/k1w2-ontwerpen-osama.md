# Inlevering ontwerpen – Osama

Hackathon 3, opdracht "k1w2 - ontwerpen". Werkproces B1-K1-W2 Ontwerpt software.
Deadline: 19 oktober 2026. Oefenbeoordeling met de SD Examentrainer.

> Dit is oefenmateriaal. De examinator bepaalt het werkelijke resultaat.

Beoordeeld op 5 november 2026, versie `main`, commit `bc168f3`.

## Scores

| Criterium | Score | Niveau |
| --- | --- | --- |
| Ontwerp (cruciaal) | 3/3 | Volledig |
| Schematechnieken | 2/3 | Grotendeels |
| Onderbouwing (cruciaal) | 2/3 | Grotendeels |
| **Totaal** | **7 van 9** | **Indicatiecijfer 8.0** |

## Feedback in het kort

- **Ontwerp:** 23 van 25 functionaliteiten uit `design.md` zitten in de schema's (92%). Mist: levelschets van level 2 en de bewegende lampen in de klasse `Lamp`. Er is geen eisenlijst, dus de telling is een schatting.
- **Schematechnieken:** drie goede technieken: klassendiagram en data-model, use case diagram, activiteitendiagrammen. Maar Gloed en Schaduwstap staan als `<<extend>>` op Kracht gebruiken. Het zijn varianten, dus daar hoort een generalisatie-pijl bij. En level 2 staat in geen enkel schema.
- **Onderbouwing:** 17 van 28 keuzes hebben een "waarom" (61%). Dat is net boven de grens: één keuze minder is een 1. Geen uitleg bij: timer loopt altijd door, krachten bij de vorm, toetsindeling, beide spelers in het portaal, kristallen, geen levens, checkpoint pas als beide langs zijn, menu-opbouw, één level per persoon, Git-werkwijze en sprints. Security is dun.
- **Los van de score:** het klassendiagram, de activiteitendiagrammen, de wireframes en het use case diagram zijn na de deadline in Git gezet, tegelijk met de code. Een examinator kan daar naar vragen.

## Inleverblok

```
SD EXAMENTRAINER — OEFENBEOORDELING
Casus:           Hackaton 3 GameJam — "Lumo & Nox" (thema Unwanted powers)
Versie:          repo lumo-and-nox, main, commit bc168f3 (3 november 2026)
Volgnr:          K1W2-322-7van9-V22O
Werkproces:      B1-K1-W2 Ontwerpt software
Beoordeeld op:   5 november 2026

Aangeleverd bewijs
  - docs/design.md: ontwerpdocument met onderbouwing
  - docs/class-diagram.md + klassendiagram en data-model (PlantUML)
  - docs/use-cases.md + use case diagram (15 use cases)
  - docs/wireframes/: 7 schermen + levelschets level 1
  - docs/activiteitendiagrammen/: 12 diagrammen (PlantUML + PNG)
  - geen aparte eisenlijst; geteld tegen design.md

Scores
  Ontwerp            3/3   Volledig
    23 van 25 functionaliteiten in de schema's (92%), kern compleet.
    Mist: levelschets level 2, bewegende lampen in klasse Lamp.
  Schematechnieken   2/3   Grotendeels
    3 technieken goed gekozen. Gloed/Schaduwstap staan als <<extend>>
    i.p.v. generalisatie; level 2 staat in geen schema.
  Onderbouwing       2/3   Grotendeels
    17 van 28 keuzes met uitleg (61%), privacy en ethiek gekoppeld
    aan het ontwerp. Security dun. Net boven de grens van 60%.

Punten:          7 van 9
Indicatiecijfer: 8.0  (voldoende vanaf 5)
Cruciale criteria: in orde

Nog niet te beoordelen: geen (eisenlijst ontbrak, telling Ontwerp is
een schatting)

Verbetersuggestie:
  Verbeter je schema's met generalisatie bij Gloed en Schaduwstap, een levelschets van level twee en bewegende lampen; dat levert één punt op.

Dit is oefenmateriaal. De examinator bepaalt het werkelijke resultaat.
```

## Antwoorden

**Welk bewijs had je nodig, en wat had je niet bij de hand?**
Ik had een ontwerp met schema’s en een onderbouwing van mijn keuzes nodig. Ik had geen eisenlijst en geen levelschets van level 2, en mijn diagrammen stonden pas na de deadline in Git.

**Ben je het eens met de beoordeling? Waarom wel of niet?**
Ja, mijn ontwerp is bijna compleet, maar bij veel keuzes heb ik niet uitgelegd waarom. Gloed en Schaduwstap zijn inderdaad varianten van Kracht gebruiken, dus generalisatie past beter dan extend.

**Wat ga je veranderen?**
Ik zet generalisatie bij Gloed en Schaduwstap, maak een levelschets van level 2 en voeg bewegende lampen toe aan de klasse Lamp. Ook schrijf ik bij de keuzes zonder uitleg waarom ik ze heb gemaakt en maak ik een eisenlijst.

## Wat we nog doen

- [ ] Generalisatie-pijl voor Gloed en Schaduwstap in het use case diagram
- [ ] Levelschets level 2 in `docs/wireframes/`
- [ ] Bewegende lampen in klasse `Lamp`
- [ ] Onderbouwing toevoegen bij de keuzes zonder "waarom" (zie Feedback in het kort)
- [ ] Security: wat gebeurt er als iemand `save.json` zelf aanpast
- [ ] Genummerde eisenlijst bovenaan `design.md`