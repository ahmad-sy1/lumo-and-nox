# Inleveringen

Na elke fase (ontwerpen, realiseren, testen) leveren we een oefenbeoordeling in. Die doen we met de SD Examentrainer. Per fase bewaren we hier het inleverblok en de antwoorden op de drie vragen.

> Dit is oefenmateriaal. De examinator bepaalt het werkelijke resultaat.

| Fase | Werkproces | Deadline | Score | Status |
| --- | --- | --- | --- | --- |
| Ontwerpen | B1-K1-W2 Ontwerpt software | 19 oktober 2026 | 8 van 9 | Beoordeeld |
| Realiseren | | | | Nog niet gedaan |
| Testen | | | | Nog niet gedaan |

---

## Fase 1: Ontwerpen (B1-K1-W2)

Beoordeeld op 2 november 2026, versie `main`, commit `9be11bd`.

### Scores

| Criterium | Score | Niveau |
| --- | --- | --- |
| Ontwerp (cruciaal) | 3/3 | Volledig |
| Schematechnieken | 3/3 | Volledig |
| Onderbouwing (cruciaal) | 2/3 | Grotendeels |
| **Totaal** | **8 van 9** | **Indicatiecijfer 9.0** |

### Feedback in het kort

- **Ontwerp:** 16 van 18 functionaliteiten zitten in de schema's (89%). Mist: levelschets van level 2 en de bewegende lampen in de klasse `Lamp`.
- **Schematechnieken:** data, gebruiker en logica hebben elk een passend schema. Tip: Gloed en Schaduwstap zijn varianten van "Kracht gebruiken". Daar hoort een generalisatie-pijl bij, geen `<<extend>>`.
- **Onderbouwing:** niet elke keuze heeft een "waarom". Geen uitleg bij: geen levens, 3 kristallen, toetsindeling, level select en menu-opbouw. Security is dun.
- **Los van de score:** maak een genummerde eisenlijst, zodat een examinator kan tellen.

### Inleverblok

```
SD EXAMENTRAINER — OEFENBEOORDELING
Casus:           Hackaton 3 GameJam — "Lumo & Nox" (thema Unwanted powers)
Versie:          repo lumo-and-nox, main, commit 9be11bd (2 november 2026)
Volgnr:          K1W2-332-8van9-S20O
Werkproces:      B1-K1-W2 Ontwerpt software
Beoordeeld op:   2 november 2026

Aangeleverd bewijs
  - docs/design.md: ontwerpdocument met onderbouwing (206 regels)
  - docs/class-diagram.md + klassendiagram en data-model (PlantUML, gerenderd)
  - docs/use-cases.md + use case diagram (PlantUML, 15 use cases)
  - docs/wireframes/: 7 schermen + levelschets level 1
  - docs/activiteitendiagrammen/: 12 activiteitendiagrammen (PlantUML + PNG)
  - geen aparte eisenlijst; geteld met casusleeswijzer H3 + design.md

Scores
  Ontwerp            3/3   Volledig
    16 van 18 functionaliteiten volledig in het ontwerp (89%), alles wat
    essentieel is erin. Mist: levelschets level 2, bewegende lamp.
  Schematechnieken   3/3   Volledig
    Data: klassendiagram/data-model. Gebruiker: use case + 8 wireframes.
    Logica: 12 PlantUML-activiteitendiagrammen. Notatie klopt.
  Onderbouwing       2/3   Grotendeels
    Geschat 70-80% van de keuzes toegelicht, met verband naar eisen,
    privacy en ethiek. Niet >90%; security is dun.

Punten:          8 van 9
Indicatiecijfer: 9.0  (voldoende vanaf 5)
Cruciale criteria: in orde

Nog niet te beoordelen: geen (eisenlijst ontbrak, telling is een schatting)

Verbetersuggestie:
  Schrijf bij elke ontwerpkeuze zonder uitleg één of twee zinnen waarom, inclusief security rond save.json; dat levert één punt op.

Dit is oefenmateriaal. De examinator bepaalt het werkelijke resultaat.
```

### Antwoorden

**Welk bewijs had je nodig, en wat had je niet bij de hand?**
Ik had het ontwerpdocument, het klassendiagram, de use cases, de wireframes en de activiteitendiagrammen nodig. Een genummerde eisenlijst en een levelschets van level 2 had ik niet, daarom is de telling een schatting.

**Ben je het eens met de beoordeling? Waarom wel of niet?**
Ja. De schema's zijn compleet, maar bij een paar keuzes staat wel wát we doen en niet waarom, en over security staat bijna niets.

**Wat ga je veranderen?**
Ik zet bij elke keuze zonder uitleg waarom we het zo doen en wat het alternatief was, en ik schrijf iets over security rond `save.json`. Ook maak ik een genummerde eisenlijst en een levelschets van level 2.

### Wat we nog doen

- [ ] Onderbouwing toevoegen: geen levens, 3 kristallen, toetsindeling, level select, menu-opbouw
- [ ] Security: wat gebeurt er als iemand `save.json` zelf aanpast
- [ ] Genummerde eisenlijst bovenaan `design.md`
- [ ] Levelschets level 2 in `docs/wireframes/`
- [ ] Bewegende lampen in klasse `Lamp`
- [ ] Generalisatie-pijl voor Gloed en Schaduwstap in het use case diagram

---

## Fase 2: Realiseren

Nog niet gedaan.

---

## Fase 3: Testen

Nog niet gedaan.
