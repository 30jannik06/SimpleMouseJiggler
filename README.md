# 🖱️ SimpleMouseJiggler

A simple Windows Forms app that prevents your machine from going idle by moving the mouse slightly at configurable intervals.

## ✨ Features

- System tray support
- Start/Stop toggle
- Interval slider (1–30 seconds)
- Zen mode (invisible movement)
- Minimizes to tray
- .ico support for tray and taskbar

## 🛠️ Requirements

- .NET 9.0 (or compatible Windows runtime)
- Windows 10 or later

## 🚀 Usage

1. Download the EXE from the releases or build it yourself:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
   ```
2. Run the EXE.  
3. Configure interval and mode as needed.  
4. Minimize to keep it in the tray.

## 🗂️ Tray Menu

- **Start**: Start jiggling
- **Stop**: Stop jiggling
- **Beenden**: Exit the app

## 📦 Portable Build

To create a single-file, portable EXE:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

No installer needed – copy to USB or any folder and run.

## ⚡ CI / GitHub Actions

This repo includes a GitHub Actions workflow that:

- Builds on push and PR
- Publishes self-contained Win64 build
- Can upload artifacts

## ❤️ License

MIT