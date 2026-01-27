# DmdSkiaFlex (starter)

A flexible SkiaSharp DMD renderer for OBS Browser Source.

## Run
```bash
dotnet run --project src/DmdSkiaFlex
```

Open:
- http://127.0.0.1:5099/

Frames:
- http://127.0.0.1:5099/frame.png?mode=pinball&scale=8
- http://127.0.0.1:5099/frame.png?mode=flat&scale=8

Set scrolling text:
- http://127.0.0.1:5099/api/text?text=HELLO%20WORLD

## Add a sprite
Drop a PNG at:
`src/DmdSkiaFlex/assets/logo.png`

It will load on startup and demo will alternate between color and mono each second.
