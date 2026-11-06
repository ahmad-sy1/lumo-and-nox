# SD EXAMENTRAINER — OEFENBEOORDELING

**Casus:** Hackaton 3 GameJam — "Lumo & Nox" (thema Unwanted powers)  
**Versie:** main b5db16a + branch feature/15-level2 af8ee15 + branch feature/32-readme cdf72f0 (stand 6 nov 2026)  
**Volgnr:** K1W3-2322-9van12-B22O  
**Werkproces:** B1-K1-W3 Realiseert (onderdelen van) software  
**Beoordeeld op:** 6 november 2026

## Aangeleverd bewijs

- GitHub-repository lumo-and-nox: 96 commits (38 van de student), 13 okt - 5 nov 2026, branches per issue, pull requests
- Code van de student: Player.cs, SwapManager.cs, HUD.cs, Level2Builder.cs, aanpassing Door.cs
- Eisen en taakverdeling: docs/design.md, docs/afspraken-player.md
- Coding conventions en Git-workflow: CLAUDE.md
- Filmpje van het werkende spel: genoemd, niet bekeken

## Scores

**Gerealiseerde functionaliteit** — 2/3 Grotendeels  
19 van 21 toegewezen functies gebouwd (90%). Mist: Nox duwt kist, kisten in Level 2. Werking niet gezien, daarom de lagere score.

**Kwaliteit opgeleverde functies** — 3/3 Volledig  
17 van 19 voldoen aan design.md (89%). Afwijking: scherm knippert niet, dood-effect is grijs in plaats van vonkjes.

**Kwaliteit code** — 2/3 Grotendeels  
Conventions en commentaar in orde. Controles niet consequent: spriteRenderer wel in Move(), niet in Die()/Respawn().

**Versiebeheer** — 2/3 Grotendeels  
Branches en PR's per issue. Workflow "nooit zelf mergen" niet gevolgd: 3 van 4 eigen code-PR's zelf gemerged.

**Punten:** 9 van 12  
**Indicatiecijfer:** 7.8 (voldoende vanaf 6)  
**Cruciale criteria:** in orde

**Nog niet te beoordelen:** werking in het spel (filmpje niet bekeken, scores op basis van de code); op tijd af (deadline niet bekend)

## Reflectie

**1. Welk bewijs had je nodig, en wat had je niet bij de hand?**  
Ik had mijn GitHub-repo, mijn code en design.md nodig. Dat had ik allemaal bij de hand.

**2. Ben je het eens met de beoordeling?**  
Grotendeels. Kisten duwen voor Nox is niet af. Mijn controles in de code zijn niet overal hetzelfde. Maar ik heb mijn eigen PR's niet zelf goedgekeurd.

**3. Wat ga je veranderen?**  
Ik maak kisten duwen af en zet een kist in Level 2. Ik laat zien dat het werkt. Ik zet de controle ook in Die() en Respawn().

## Verbetersuggestie

Bouw kisten duwen voor Nox af, zet een kist in Level 2 en laat het werken zien. Dat levert 1 punt op.

---

*Dit is oefenmateriaal. De examinator bepaalt het werkelijke resultaat.*