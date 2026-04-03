# 🚀 LibLGFs.SteamLocal
**A lightweight, high-performance, production-ready .NET library for parsing Steam local files, game libraries, installation information, and ACF manifests.**

No login required, no network connection, no Steam running — pure local file reading.

## ✨ Features
- Automatically locate the Steam installation directory (Registry + common path)
- Scan all Steam library folders (automatic full-disk detection)
- Parse ACF manifest files of all installed games
- Retrieve game AppId, name, installation path, size, language, and update status
- Automatically calculate the actual game installation directory
- Fully asynchronous, high-performance, and non-blocking UI
- Windows-only platform, stable and error-free

## 📦 Installation (NuGet)
```
Install-Package LibLGFs.SteamLocal.Release
```
Or
```
dotnet add package LibLGFs.SteamLocal.Release
```

## 🔎 Usage Examples
### Get All Installed Games
```csharp
// Get all installed Steam games
var games = await LibLGFs.SteamLocal.SteamLibrary.SteamApps.EnumInstalledGamesAsync();

foreach (var game in games)
{
    Console.WriteLine($"AppId: {game.AppState?.AppId}");
    Console.WriteLine($"Name: {game.AppState?.Name}");
    Console.WriteLine($"Installation Directory: {game.GameInstalledDir}");
    Console.WriteLine($"ACF File: {game.File}");
    Console.WriteLine("---");
}
```

### Check if a Specific Game is Installed (by AppId)
```csharp
var games = await LibLGFs.SteamLocal.SteamLibrary.SteamApps.EnumInstalledGamesAsync();

var deadCells = games.FirstOrDefault(g => g.AppState?.AppId == "588650");

if (deadCells != null)
{
    Console.WriteLine($"Installed: {deadCells.AppState.Name}");
    Console.WriteLine($"Path: {deadCells.GameInstalledDir}");
}
else
{
    Console.WriteLine("Game not installed");
}
```

### Scan Only the Default Steam Library
```csharp
var games = await SteamApps.EnumInstalledGamesForSteamLibraryAsync();
```

### Scan Steam Libraries on a Specific Drive
```csharp
var gamesD = await SteamApps.EnumInstalledGamesForDriverAsync(@"D:\");
```

## 🧩 Supported Files for Parsing
- `appmanifest_*.acf` files parsing
- Automatic Steam installation path detection

## 🎯 Supported Platforms
- **Windows Only** (x86/x64)
- .NET 10

## 📌 Author
control0forver (TheLGF)
