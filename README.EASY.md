# LibLGFs.SteamLocal

### Usage
Add to your project
```xml
<ItemGroup Condition="'$(Configuration)' == 'Release'">
  <PackageReference Include="LibLGFs.SteamLocal.Release" Version="1.0.0.4" />
</ItemGroup>
<ItemGroup Condition="'$(Configuration)' == 'Debug'"> 
  <PackageReference Include="LibLGFs.SteamLocal.Debug" Version="1.0.0.4" />
</ItemGroup>
```

### Example
```csharp
// Example - Check if game "Dead Cells" is installed

using System.Diagnostics;

bool found = false;
var installedGames = await LibLGFs.SteamLocal.SteamLibrary.SteamApps.EnumInstalledGamesAsync();
foreach (var installedGame in installedGames)
{
    if (installedGame.AppState?.AppId?.Equals("588650") ?? false)
        goto found;
    else // Way 2 - Check the Game Name
        if (installedGame.AppState?.Name?.Contains("Dead Cells") ?? false)
            goto found;

    continue;

found:
    Console.WriteLine($"[appid: {installedGame.AppState.AppId}] {installedGame.AppState.Name} Installed at \"{installedGame.GameInstalledDir}\" (steam acf: {installedGame.File})");
    found = true;
    Debugger.Break();
}
if (!found)
    Console.WriteLine("Game not found");
```