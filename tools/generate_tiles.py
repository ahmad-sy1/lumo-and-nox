"""Lumo & Nox – tileset generator.

Tekent alle omgevings-sprites als pixel art (16 px tegels) in het palet van Lumo en Nox.
Licht = goed: zacht, rond, warm, goudrandjes.
Schaduw = kwaad: puntig, doornen, koud, harde randen.

Gebruik:  python3 tools/generate_tiles.py [uitvoermap]
Standaard uitvoer: Assets/Art/Tiles/
"""
import os
import sys

from PIL import Image

T = 16  # tegelgrootte in pixels

# --- Palet -----------------------------------------------------------------
C = {
    # Licht (goed)
    "nacht_l": (42, 35, 64, 255),       # #2a2340
    "steen": (232, 217, 176, 255),      # #e8d9b0
    "steen_h": (247, 238, 212, 255),    # highlight
    "steen_s": (201, 169, 106, 255),    # #c9a96a
    "steen_d": (150, 118, 66, 255),     # donkere rand
    "goud": (242, 193, 78, 255),        # #f2c14e
    "goud_d": (190, 132, 40, 255),
    "gloed": (255, 233, 168, 255),      # #ffe9a8
    "mos": (111, 143, 90, 255),
    "mos_d": (77, 107, 63, 255),
    # Schaduw (kwaad)
    "nacht_s": (20, 17, 42, 255),       # #14112a
    "rots": (59, 47, 92, 255),          # #3b2f5c
    "rots_h": (82, 66, 124, 255),
    "rots_s": (42, 33, 71, 255),        # #2a2147
    "rots_d": (28, 22, 50, 255),
    "doorn": (107, 79, 160, 255),       # #6b4fa0
    "doorn_h": (150, 118, 206, 255),
    "poel": (11, 9, 24, 255),           # #0b0918
    # Gedeeld
    "kristal": (143, 211, 255, 255),    # #8fd3ff
    "kristal_h": (201, 240, 255, 255),  # #c9f0ff
    "kristal_d": (74, 138, 196, 255),
    "hout": (122, 86, 58, 255),
    "hout_h": (158, 116, 78, 255),
    "hout_d": (82, 56, 40, 255),
    "ijzer": (92, 96, 120, 255),
    "ijzer_h": (140, 146, 170, 255),
}
CLEAR = (0, 0, 0, 0)


def new(w=T, h=T):
    return Image.new("RGBA", (w, h), CLEAR)


def px(img, x, y, c):
    if 0 <= x < img.width and 0 <= y < img.height:
        img.putpixel((x, y), C[c] if isinstance(c, str) else c)


def rect(img, x0, y0, x1, y1, c):
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            px(img, x, y, c)


def alpha(c, a):
    r, g, b, _ = C[c]
    return (r, g, b, a)


# --- Licht-tegels ------------------------------------------------------------
def vloer_licht_top():
    im = new()
    rect(im, 0, 0, 15, 15, "steen")
    rect(im, 0, 0, 15, 1, "steen_h")
    rect(im, 0, 2, 15, 2, "goud")            # goudrand
    for x in (0, 1, 8, 9):                    # zachte bloksteen-voegen
        pass
    rect(im, 7, 3, 7, 8, "steen_s")
    rect(im, 0, 9, 15, 9, "steen_s")
    rect(im, 3, 10, 3, 15, "steen_s")
    rect(im, 12, 10, 12, 15, "steen_s")
    rect(im, 0, 15, 15, 15, "steen_s")
    for x, y in ((2, 5), (11, 6), (6, 12), (14, 12)):
        px(im, x, y, "steen_h")
    return im


def vloer_licht_vul():
    im = new()
    rect(im, 0, 0, 15, 15, "steen_s")
    rect(im, 0, 7, 15, 7, "steen_d")
    rect(im, 5, 0, 5, 6, "steen_d")
    rect(im, 12, 8, 12, 15, "steen_d")
    rect(im, 0, 15, 15, 15, "steen_d")
    for x, y in ((2, 3), (9, 2), (7, 11), (14, 4)):
        px(im, x, y, "steen")
    return im


def platform_licht():
    im = new()
    rect(im, 0, 0, 15, 5, "steen")
    rect(im, 0, 0, 15, 0, "steen_h")
    rect(im, 0, 1, 15, 1, "goud")
    rect(im, 0, 5, 15, 5, "steen_s")
    rect(im, 0, 6, 15, 6, "steen_d")
    # zachte ronde hoeken
    px(im, 0, 0, CLEAR); px(im, 15, 0, CLEAR)
    px(im, 0, 6, CLEAR); px(im, 15, 6, CLEAR)
    for x in (3, 8, 12):
        px(im, x, 3, "steen_h")
    return im


def lichtplatform(zichtbaar):
    im = new()
    if zichtbaar:
        rect(im, 0, 0, 15, 5, alpha("gloed", 230))
        rect(im, 0, 0, 15, 0, alpha("goud", 255))
        rect(im, 0, 5, 15, 5, alpha("goud", 255))
        for x in range(1, 15, 3):
            px(im, x, 2, "goud")
            px(im, x + 1, 3, "goud")
    else:  # stippellijn – niet betreedbaar
        for x in range(0, 16, 3):
            px(im, x, 0, alpha("gloed", 140))
            px(im, x, 5, alpha("gloed", 140))
        px(im, 0, 2, alpha("gloed", 140)); px(im, 15, 3, alpha("gloed", 140))
    return im


# --- Schaduw-tegels ----------------------------------------------------------
def vloer_schaduw_top():
    im = new()
    rect(im, 0, 3, 15, 15, "rots")
    rect(im, 0, 3, 15, 3, "rots_h")
    # gekartelde bovenkant met doornpuntjes
    for x in (1, 6, 11):
        px(im, x, 2, "doorn"); px(im, x + 1, 2, "doorn"); px(im, x + 1, 1, "doorn_h"); px(im, x + 1, 0, "doorn")
    for x in (4, 9, 14):
        px(im, x, 2, "rots_h")
    # harde, scheve breuklijnen
    for i in range(5):
        px(im, 3 + i, 6 + i, "rots_s")
    for i in range(4):
        px(im, 13 - i, 9 + i, "rots_s")
    rect(im, 0, 15, 15, 15, "rots_s")
    return im


def vloer_schaduw_vul():
    im = new()
    rect(im, 0, 0, 15, 15, "rots_s")
    for i in range(6):
        px(im, 2 + i, 1 + i, "rots_d")
    for i in range(5):
        px(im, 14 - i, 6 + i, "rots_d")
    rect(im, 0, 15, 15, 15, "rots_d")
    for x, y in ((11, 2), (4, 12), (9, 13)):
        px(im, x, y, "rots")
    return im


def platform_schaduw():
    im = new()
    rect(im, 0, 1, 15, 5, "rots")
    rect(im, 0, 1, 15, 1, "rots_h")
    rect(im, 0, 5, 15, 5, "rots_s")
    # puntjes naar beneden (scherp)
    for x in (1, 5, 9, 13):
        px(im, x, 6, "rots_s"); px(im, x + 1, 6, "rots_s"); px(im, x, 7, "rots_d")
    px(im, 3, 0, "doorn"); px(im, 11, 0, "doorn")
    return im


def schaduwmuur():
    im = new()
    rect(im, 0, 0, 15, 15, alpha("doorn", 120))
    rect(im, 0, 0, 0, 15, alpha("doorn_h", 200))
    rect(im, 15, 0, 15, 15, alpha("doorn_h", 200))
    # wervelend schaduwpatroon
    for y in range(0, 16, 4):
        for x in range(1, 15):
            if (x + y) % 5 == 0:
                px(im, x, y, alpha("nacht_s", 220))
                px(im, x, y + 1, alpha("nacht_s", 160))
    return im


def donkere_poel():
    im = new()
    rect(im, 0, 4, 15, 15, "poel")
    # olieachtig oppervlak, glimmende rimpels
    rect(im, 0, 4, 15, 4, "rots_s")
    for x in (2, 3, 9, 10, 11):
        px(im, x, 6, "rots")
    px(im, 6, 9, "doorn"); px(im, 13, 10, "doorn")
    # bubbels/oogjes
    px(im, 5, 12, "rots_s"); px(im, 12, 13, "rots_s")
    return im


def stekels():
    im = new()
    for base in (0, 5, 10):
        for h in range(12):
            w = (11 - h) // 4
            for x in range(base + 3 - w, base + 3 + w + 1):
                left = x < base + 3
                px(im, x, 15 - h, "rots_h" if left else "rots_s")
        px(im, base + 3, 4, "doorn_h")
        px(im, base + 3, 5, "doorn")
    rect(im, 0, 15, 15, 15, "rots_d")
    return im


# --- Puzzel-objecten -----------------------------------------------------------
def kist():
    im = new()
    rect(im, 1, 1, 14, 15, "hout")
    rect(im, 1, 1, 14, 1, "hout_h")
    rect(im, 1, 1, 1, 15, "hout_h")
    rect(im, 14, 1, 14, 15, "hout_d")
    rect(im, 1, 15, 14, 15, "hout_d")
    rect(im, 2, 5, 13, 5, "hout_d")
    rect(im, 2, 10, 13, 10, "hout_d")
    for i in range(10):                      # kruisbalk
        px(im, 3 + i, 13 - i, "hout_d")
    for x, y in ((2, 2), (13, 2), (2, 14), (13, 14)):
        px(im, x, y, "ijzer_h")
    return im


def kristal():
    im = new()
    shape = [
        "......xx......",
        ".....xhhx.....",
        "....xhhkkx....",
        "...xhhkkkdx...",
        "...xhkkkkdx...",
        "..xhkkkkkddx..",
        "..xhkkkkkddx..",
        "..xkkkkkkddx..",
        "...xkkkkddx...",
        "....xkkkdx....",
        ".....xkdx.....",
        "......xx......",
    ]
    cmap = {"x": "kristal_d", "h": "kristal_h", "k": "kristal", "d": "kristal_d"}
    for y, row in enumerate(shape):
        for x, ch in enumerate(row):
            if ch in cmap:
                px(im, x + 1, y + 2, cmap[ch])
    px(im, 13, 2, "kristal_h"); px(im, 2, 5, "kristal_h")  # glinstering
    return im


def icoon_zon():
    im = new()
    shape = [
        ".......x........",
        ".x.....x.....x..",
        "..x...xxx...x...",
        "...x.xgggx.x....",
        ".....gggggg.....",
        "....gghhhggx....",
        "xx..gghhgggg.xx.",
        "xx..gggggggg.xx.",
        "....gggggggd....",
        ".....gggggd.....",
        "...x.xgggx.x....",
        "..x...xxx...x...",
        ".x.....x.....x..",
        ".......x........",
    ]
    cmap = {"x": "goud", "g": "goud", "h": "gloed", "d": "goud_d"}
    for y, row in enumerate(shape):
        for x, ch in enumerate(row):
            if ch in cmap:
                px(im, x, y + 1, cmap[ch])
    return im


def icoon_maan():
    im = new()
    shape = [
        "......hhhh......",
        "....hhhhh.......",
        "...hhhhh........",
        "..hhhhh.........",
        "..hhhhh.........",
        ".hhhhhh.........",
        ".hhhhhh.........",
        ".hhhhhh.........",
        "..hhhhhh......dd",
        "..hhhhhhh....ddd",
        "...hhhhhhhhhddd.",
        "....ddhhhhhddd..",
        "......dddddd....",
    ]
    cmap = {"h": "doornH", "d": "doorn"}
    for y, row in enumerate(shape):
        for x, ch in enumerate(row):
            if ch in cmap:
                px(im, x, y + 1, "doorn_h" if ch == "h" else "doorn")
    return im


def hendel(stand):
    im = new()
    rect(im, 3, 12, 12, 15, "ijzer")
    rect(im, 3, 12, 12, 12, "ijzer_h")
    dx = -1 if stand == "links" else 1
    for i in range(7):
        px(im, 8 + dx * (i // 2), 11 - i, "hout_h" if i % 2 else "hout")
    kx = 8 + dx * 3
    rect(im, kx - 1, 3, kx + 1, 5, "goud" if stand == "rechts" else "doorn")
    px(im, kx - 1, 3, "gloed" if stand == "rechts" else "doorn_h")
    return im


def drukknop(ingedrukt):
    im = new()
    rect(im, 1, 13, 14, 15, "ijzer")
    rect(im, 1, 13, 14, 13, "ijzer_h")
    top = 11 if ingedrukt else 9
    rect(im, 3, top, 12, 12, "goud" if ingedrukt else "kristal")
    rect(im, 3, top, 12, top, "gloed" if ingedrukt else "kristal_h")
    return im


def deur(open_):
    im = new(T, 2 * T)
    if open_:
        rect(im, 0, 0, 1, 31, "ijzer")
        rect(im, 14, 0, 15, 31, "ijzer")
        rect(im, 0, 0, 15, 1, "ijzer")
        rect(im, 2, 2, 13, 31, alpha("nacht_s", 90))
        for y in (0, 31):
            px(im, 0, y, "ijzer_h")
        return im
    rect(im, 0, 0, 15, 31, "rots")
    rect(im, 0, 0, 15, 1, "ijzer_h")
    rect(im, 0, 0, 1, 31, "ijzer")
    rect(im, 14, 0, 15, 31, "ijzer")
    for y in (8, 16, 24):
        rect(im, 2, y, 13, y, "rots_s")
    for x in (5, 10):
        rect(im, x, 2, x, 31, "rots_s")
    rect(im, 6, 14, 9, 17, "kristal_d")     # slot
    px(im, 7, 15, "kristal_h")
    return im


def lamp():
    im = new()
    rect(im, 7, 0, 8, 4, "ijzer")
    rect(im, 3, 5, 12, 6, "ijzer_h")
    rect(im, 4, 7, 11, 11, "goud")
    rect(im, 5, 8, 10, 10, "gloed")
    rect(im, 3, 12, 12, 13, "ijzer")
    return im


def lamp_uit():
    im = lamp()
    rect(im, 4, 7, 11, 11, "rots_s")
    rect(im, 5, 8, 10, 10, "rots")
    return im


def lichtstraal():
    """Tegelbare verticale lichtbundel (stretch in Unity). Warm goud, fel in het midden."""
    im = new()
    for y in range(T):
        for x in range(T):
            mid = abs(x - 7.5)
            if mid > 7:
                continue
            if mid < 2:
                c, a = C["gloed"], 210
            elif mid < 4.5:
                c, a = C["goud"], 150
            else:
                c, a = C["goud_d"], 80
            if x in (5, 10) and (y // 2) % 3 == 0:   # zachte glinstering
                c, a = C["gloed"], 230
            im.putpixel((x, y), (*c[:3], a))
    return im


def lantaarn(aan):
    im = new(T, 2 * T)
    rect(im, 7, 10, 8, 31, "ijzer")          # paal
    rect(im, 5, 30, 10, 31, "ijzer_h")
    rect(im, 4, 2, 11, 3, "ijzer_h")         # kap
    rect(im, 6, 0, 9, 1, "ijzer")
    rect(im, 4, 4, 4, 10, "ijzer")
    rect(im, 11, 4, 11, 10, "ijzer")
    rect(im, 4, 10, 11, 10, "ijzer")
    if aan:
        rect(im, 5, 4, 10, 9, "goud")
        rect(im, 6, 5, 9, 8, "gloed")
        for x, y in ((2, 4), (13, 5), (3, 9), (12, 9), (7, 13)):
            px(im, x, y, alpha("gloed", 150))
    else:
        rect(im, 5, 4, 10, 9, "rots_s")
        rect(im, 6, 5, 9, 6, "rots")
    return im


def portaal():
    im = new(2 * T, 2 * T)
    cx, cy = 15.5, 17
    for y in range(32):
        for x in range(32):
            dx, dy = (x - cx) / 13, (y - cy) / 15
            d = dx * dx + dy * dy
            if y > 31:
                continue
            if d <= 1.0:
                if d > 0.78:
                    im.putpixel((x, y), C["steen"] if x < cx else C["rots"])
                elif d > 0.62:
                    im.putpixel((x, y), C["goud"] if x < cx else C["doorn"])
                else:
                    # midden: licht en schaduw draaien in elkaar
                    band = int((x + y * 0.6) / 3) % 2
                    im.putpixel((x, y), (*C["gloed"][:3], 200) if band == 0 else (*C["nacht_s"][:3], 220))
    rect(im, 4, 30, 27, 31, "steen_s")
    rect(im, 16, 30, 27, 31, "rots_s")
    px(im, 15, 6, "kristal_h"); px(im, 16, 6, "kristal")
    return im


SPRITES = {
    # licht
    "licht_vloer_top": vloer_licht_top,
    "licht_vloer_vul": vloer_licht_vul,
    "licht_platform": platform_licht,
    "lichtplatform_zichtbaar": lambda: lichtplatform(True),
    "lichtplatform_verborgen": lambda: lichtplatform(False),
    "lamp_aan": lamp,
    "lamp_uit": lamp_uit,
    "lichtstraal": lichtstraal,
    "lantaarn_uit": lambda: lantaarn(False),
    "lantaarn_aan": lambda: lantaarn(True),
    # schaduw
    "schaduw_vloer_top": vloer_schaduw_top,
    "schaduw_vloer_vul": vloer_schaduw_vul,
    "schaduw_platform": platform_schaduw,
    "schaduwmuur": schaduwmuur,
    "donkere_poel": donkere_poel,
    "stekels": stekels,
    # puzzel
    "kist": kist,
    "kristal": kristal,
    "hendel_links": lambda: hendel("links"),
    "hendel_rechts": lambda: hendel("rechts"),
    "drukknop_omhoog": lambda: drukknop(False),
    "drukknop_ingedrukt": lambda: drukknop(True),
    "deur_dicht": lambda: deur(False),
    "deur_open": lambda: deur(True),
    "portaal": portaal,
    # HUD
    "icoon_zon": icoon_zon,
    "icoon_maan": icoon_maan,
}


def main():
    out = sys.argv[1] if len(sys.argv) > 1 else os.path.join("Assets", "Art", "Tiles")
    os.makedirs(out, exist_ok=True)
    for name, fn in SPRITES.items():
        fn().save(os.path.join(out, f"{name}.png"))
    print(f"{len(SPRITES)} sprites geschreven naar {out}")


if __name__ == "__main__":
    main()
