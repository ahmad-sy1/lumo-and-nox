# Lumo & Nox – Activiteitendiagrammen (PlantUML)

Elk blok hieronder is een compleet diagram. Kopieer het van `@startuml` tot en met `@enduml` en plak het op plantuml.com om het te renderen.

## 00 – Een level spelen (overzicht)

```plantuml
@startuml 00_level_spelen
title 00 – Een level spelen (overzicht)

|GameManager|
start
:Level-scene laden;
|LevelManager|
:Tijd, deaths en kristallen op 0 zetten;
:Respawnpunt = startpunt;
fork
  |Player|
  :Lumo verschijnt in licht-vorm\nNox verschijnt in schaduw-vorm;
fork again
  |HUD / Menu|
  :HUD tonen\n(timer, vorm-iconen, kristallen 0/3);
end fork
|SwapManager|
:Wissel-timer starten (zie 01);
fork
  |Player|
  repeat
    :Invoer van beide spelers verwerken\n(lopen, springen, kracht, hendel);
    note right
      Details per actie:
      02 doodgaan, 03 checkpoint,
      04a gloed, 04b schaduwstap, 05 kist,
      06a hendel, 06b drukknop,
      07 kristal, 09 pauze
    end note
  repeat while (Staan beide spelers\ntegelijk in het portaal?) is (nee)
  -> ja;
fork again
  |SwapManager|
  repeat
    :Timer aftellen en\nop tijd wisselen (zie 01);
  repeat while (Level nog bezig?) is (ja)
  -> nee;
end fork
|LevelManager|
:Level voltooid (zie 08);
stop
@enduml
```

## 01 – Wissel van vorm

```plantuml
@startuml 01_wissel
title 01 – Wissel van vorm

|SwapManager|
start
:Rondetijd bepalen\n(Level 1: 30 s, Level 2: 20 s);
while (Level nog bezig?) is (ja)
  :Timer = rondetijd;
  :Waarschuwing = uit;
  repeat
    if (Spel gepauzeerd?) then (ja)
      :Timer staat stil;
    else (nee)
      :Timer aftellen;
      |HUD / Menu|
      :Timerbalk en seconden bijwerken;
      |SwapManager|
      if (Timer <= 3 s en\nwaarschuwing nog uit?) then (ja)
        :Waarschuwing = aan;
        fork
          :Piep afspelen;
        fork again
          |HUD / Menu|
          :Timerbalk rood maken;
          if (Knipperen aan\nin Opties?) then (ja)
            :Scherm rustig laten knipperen;
          else (nee)
          endif
        end fork
      else (nee)
      endif
    endif
    |SwapManager|
  repeat while (Timer > 0?) is (ja)
  -> nee;
  fork
    |Player|
    :Lumo: licht <-> schaduw;
  fork again
    |Player|
    :Nox: licht <-> schaduw;
  fork again
    |HUD / Menu|
    :Vorm-iconen wisselen\n(zon <-> maan);
  end fork
  |Player|
  :Krachten en zwaktes van\nde nieuwe vorm worden actief;
  |LightZone / DarkZone|
  :Controleren of een speler nu\nin de verkeerde zone staat (zie 02);
  |SwapManager|
endwhile (nee)
stop
@enduml
```

## 02 – Doodgaan en respawnen

```plantuml
@startuml 02_doodgaan_respawn
title 02 – Doodgaan en respawnen

|LightZone / DarkZone|
start
:Speler raakt een gevaar\nof er vindt een wissel plaats;
if (Stekels of afgrond?) then (ja)
elseif (Lichtstraal en speler\nin schaduw-vorm?) then (ja)
elseif (Donkere poel en speler\nin licht-vorm?) then (ja)
else (nee)
  :Speler is veilig;
  stop
endif
|Player|
:Speler valt uit elkaar\nin vonkjes (licht) of rook (schaduw);
|LevelManager|
:Deaths + 1;
:Besturing van beide spelers\nkort uitzetten;
if (Is er een checkpoint\ngeactiveerd?) then (ja)
  :Respawnpunt = laatste checkpoint;
else (nee)
  :Respawnpunt = startpunt;
endif
fork
  |Player|
  :Lumo naar respawnpunt,\nin licht-vorm;
fork again
  |Player|
  :Nox naar respawnpunt,\nin schaduw-vorm;
end fork
note right
  Je faalt samen:
  als één speler doodgaat,
  beginnen beide opnieuw.
end note
|SwapManager|
:Wissel-timer opnieuw starten;
|LevelManager|
:Besturing weer aanzetten;
stop
@enduml
```

## 03 – Checkpoint activeren

```plantuml
@startuml 03_checkpoint
title 03 – Checkpoint activeren

|Player|
start
:Speler loopt langs de lantaarn;
|Checkpoint|
if (Lantaarn al aan?) then (ja)
  :Niets doen;
  stop
else (nee)
endif
:Deze speler markeren als 'gepasseerd';
if (Hebben beide spelers\nde lantaarn gepasseerd?) then (nee)
  :Lantaarn gloeit zwak\n(wacht op de andere speler);
  stop
else (ja)
endif
:Lantaarn gaat branden;
|LevelManager|
:Checkpoint opslaan\nals nieuw respawnpunt;
|SwapManager|
:Wissel-timer opnieuw starten;
stop
@enduml
```

## 04a – Kracht in licht-vorm: Gloed

```plantuml
@startuml 04a_gloed
title 04a – Kracht in licht-vorm: Gloed

|Player|
start
:Speler houdt krachtknop ingedrukt\n(Lumo: S, Nox: pijltje omlaag);
if (Is de speler in licht-vorm?) then (nee)
  :Niets gebeurt\n(zie 04b voor schaduw-vorm);
  stop
else (ja)
endif
:Gloed aan (licht om de speler);
|LightPlatform|
:Lichtplatforms in de buurt\nzichtbaar en betreedbaar maken;
|Player|
repeat
  :Speler beweegt, Gloed blijft aan;
repeat while (Knop nog ingedrukt en\nvorm nog licht?) is (ja)
-> nee;
:Gloed uit;
|LightPlatform|
:Lichtplatforms weer onzichtbaar\nen niet betreedbaar;
|Player|
if (Stond de speler\nop een lichtplatform?) then (ja)
  :Speler valt;
else (nee)
endif
stop
@enduml
```

## 04b – Kracht in schaduw-vorm: Schaduwstap

```plantuml
@startuml 04b_schaduwstap
title 04b – Kracht in schaduw-vorm: Schaduwstap

|Player|
start
:Speler houdt krachtknop ingedrukt\n(Lumo: S, Nox: pijltje omlaag);
if (Is de speler in schaduw-vorm?) then (nee)
  :Niets gebeurt\n(zie 04a voor licht-vorm);
  stop
else (ja)
endif
:Schaduwstap aan;
|ShadowWall|
:Schaduwmuren worden\ndoorloopbaar voor deze speler;
|Player|
repeat
  :Speler beweegt, mag door schaduwmuren;
repeat while (Knop nog ingedrukt en\nvorm nog schaduw?) is (ja)
-> nee;
:Schaduwstap uit;
|ShadowWall|
:Schaduwmuren weer dicht;
if (Staat de speler\nnog in een muur?) then (ja)
  |Player|
  :Speler naar de dichtstbijzijnde\nvrije kant duwen;
else (nee)
endif
stop
@enduml
```

## 05 – Kist duwen (alleen Nox)

```plantuml
@startuml 05_kist_duwen
title 05 – Kist duwen (alleen Nox)

|Player|
start
:Speler loopt tegen een kist aan;
if (Is de speler Nox?) then (nee)
  :Kist blijft staan\n(Lumo kan niet duwen);
  stop
else (ja)
endif
:Duw-animatie afspelen;
|PushBox|
repeat
  :Kist schuift mee in de looprichting;
  if (Kist staat in een lichtstraal?) then (ja)
    |Lamp|
    :Lichtstraal stopt bij de kist;
    |LightZone / DarkZone|
    :LightZone achter de kist uitzetten\n(daar is nu schaduw);
  else (nee)
    |LightZone / DarkZone|
    :LightZone van de lamp\nweer volledig aanzetten;
  endif
  |PushBox|
repeat while (Duwt Nox nog\nen is er ruimte?) is (ja)
-> nee;
:Kist blijft op zijn plek staan;
stop
@enduml
```

## 06a – Hendel gebruiken

```plantuml
@startuml 06a_hendel
title 06a – Hendel gebruiken

|Player|
start
:Speler staat bij een hendel en drukt\nop E (Lumo) of rechter Shift (Nox);
|Lever / PressButton|
:Hendel omzetten;
if (Waar is de hendel\naan gekoppeld?) then (lamp)
  |Lamp|
  :Lamp aan of uit zetten;
  |LightZone / DarkZone|
  :LightZone van de lamp\naan of uit zetten;
  note right
    Lamp uit = schaduw,
    dus veilig voor de schaduw-vorm.
  end note
else (deur)
  |Door|
  :Deur open of dicht;
endif
stop
@enduml
```

## 06b – Drukknop en deur

```plantuml
@startuml 06b_drukknop
title 06b – Drukknop en deur

|Player|
start
:Speler gaat op de drukknop staan;
|Lever / PressButton|
:Knop ingedrukt;
|Door|
:Deur gaat open;
|Lever / PressButton|
while (Staat er nog een\nspeler op de knop?) is (ja)
  |Door|
  :Deur blijft open;
  |Lever / PressButton|
endwhile (nee)
:Knop komt omhoog;
|Door|
while (Staat er een speler\nin de deuropening?) is (ja)
  :Wachten met sluiten;
endwhile (nee)
:Deur gaat dicht;
note right
  De deur sluit nooit op een speler.
  Samenwerken: de één blijft op de knop
  staan terwijl de ander erdoor gaat.
end note
stop
@enduml
```

## 07 – Kristal oppakken

```plantuml
@startuml 07_kristal
title 07 – Kristal oppakken

|Player|
start
:Speler raakt een kristal;
|Crystal|
if (Al opgepakt?) then (ja)
  stop
else (nee)
endif
:Kristal verdwijnt met een sprankel\nen geluidje;
|LevelManager|
:Kristallen + 1;
|HUD / Menu|
:Teller bijwerken (bijv. 1/3);
note right
  Een opgepakt kristal blijft
  opgepakt, ook als de spelers
  daarna doodgaan.
end note
stop
@enduml
```

## 08 – Level voltooien

```plantuml
@startuml 08_level_voltooien
title 08 – Level voltooien

|Player|
start
:Speler stapt in het portaal;
|LevelManager|
if (Staat de andere speler\nook in het portaal?) then (nee)
  :Portaal licht half op\n(wacht op de andere speler);
  stop
else (ja)
endif
fork
  :Besturing uitzetten;
fork again
  |SwapManager|
  :Wissel-timer stoppen;
end fork
|LevelManager|
:Tijd, kristallen en deaths verzamelen;
|GameManager|
:Level opslaan als 'gehaald';
:Volgend level ontgrendelen;
if (Betere tijd of meer kristallen\ndan opgeslagen?) then (ja)
  :Beste resultaat lokaal opslaan;
else (nee)
endif
|HUD / Menu|
:HUD verbergen;
:Winscherm tonen\n(tijd, kristallen, deaths);
|GameManager|
switch (Welke knop kiest de speler?)
case (Volgend level)
  if (Was dit het laatste level?) then (ja)
    :Hoofdmenu laden;
  else (nee)
    :Volgend level laden (zie 00);
  endif
case (Opnieuw)
  :Zelfde level opnieuw laden;
case (Hoofdmenu)
  :Hoofdmenu laden;
endswitch
stop
@enduml
```

## 09 – Pauzemenu

```plantuml
@startuml 09_pauze
title 09 – Pauzemenu

|Player|
start
:Een van de spelers drukt op Esc;
|GameManager|
:Spel bevriezen (tijd staat stil);
|SwapManager|
:Wissel-timer pauzeren;
|HUD / Menu|
:HUD verbergen en spel wazig maken;
:Pauzemenu tonen;
while (Kiest de speler Opties?) is (ja)
  :Optiesscherm tonen;
  :Speler past instellingen aan\n(volume, knipperen);
  |GameManager|
  :Instellingen lokaal opslaan;
  |HUD / Menu|
  :Terug -> pauzemenu tonen;
endwhile (nee)
|GameManager|
switch (Welke knop kiest de speler?)
case (Verder of Esc)
  :Pauzemenu sluiten en HUD tonen;
  :Spel en wissel-timer hervatten;
case (Opnieuw)
  :Level opnieuw laden vanaf het begin\n(tijd, kristallen en deaths op 0);
case (Hoofdmenu)
  :Hoofdmenu laden\n(voortgang van dit level vervalt);
endswitch
stop
@enduml
```
