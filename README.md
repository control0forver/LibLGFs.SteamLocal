# LibLGFs.SteamLocal

### Example
```csharp
using System.Diagnostics;

bool found = false;
var installedGames = await LibLGFs.SteamLocal.SteamLibrary.SteamApps.EnumInstalledGamesAsync();
foreach (var installedGame in installedGames)
{
    if (installedGame.AppState?.AppId?.Equals("Dead Cells") ?? false)
        goto found;
    else // Way 2 - Check the Game Name
        if (installedGame.AppState?.Name?.Contains("Dead Cells") ?? false)
            goto found;

    continue;

found:
    Console.WriteLine($"Dead Cells Installed at \"{installedGame.GameInstalledDir}\" (steam acf: {installedGame.File})");
    found = true;
    Debugger.Break();
}
if (!found)
    Console.WriteLine("Game not found");
```