//Console.WriteLine(
//    string.Join( Environment.NewLine,
//        DriveInfo.GetDrives().Select(d =>
//            $"Driver \"{d.Name}\"\n" +
//            $"  Type: {d.DriveType}\n" +
//            $"  Name: {d.VolumeLabel}\n"
//        )
//    )
//);

//using System.Diagnostics;
//using static LibLGFs.SteamLocal.SteamLibrary.SteamApps;
//
//using var stream = File.OpenRead(@"C:\Program Files (x86)\Steam\steamapps\appmanifest_240.acf");
//var acf = ValveKeyValue.KVSerializer.Create(ValveKeyValue.KVSerializationFormat.KeyValues1Text).Deserialize<Acf.CAppState>(stream);
//Debugger.Break();

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

Console.WriteLine("[ Test Finished - Press ANY KEY TO 9^!IH*(@Y(O12345678W*Q( ]");
Console.ReadKey(true);