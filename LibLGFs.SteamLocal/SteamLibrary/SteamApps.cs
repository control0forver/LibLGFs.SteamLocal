using System.Runtime.Versioning;

namespace LibLGFs.SteamLocal.SteamLibrary;

public static class SteamApps
{
    /// <summary>
    /// Acf Model for ValveKeyValue only
    /// </summary>
    public class Acf
    {
        public class CAppState
        {
            public class CInstalledDepot
            {
                [ValveKeyValue.KVProperty("manifest")]
                public string? Manifest { get; set; }
                [ValveKeyValue.KVProperty("size")]
                public string? Size { get; set; }
            }
            public class CUserConfig
            {
                [ValveKeyValue.KVProperty("language")]
                public string? Language { get; set; }
                public string? BetaKey { get; set; }
            }
            public class CMountedConfig
            {
                [ValveKeyValue.KVProperty("language")]
                public string? Language { get; set; }
                public string? BetaKey { get; set; }
            }

            [ValveKeyValue.KVProperty("appid")]
            public string? AppId { get; set; }
            [ValveKeyValue.KVProperty("universe")]
            public string? Universe { get; set; }
            public string? LauncherPath { get; set; }
            [ValveKeyValue.KVProperty("name")]
            public string? Name { get; set; }
            public string? StateFlags { get; set; }
            [ValveKeyValue.KVProperty("installdir")]
            public string? Installdir { get; set; }
            public string? LastUpdated { get; set; }
            public string? LastPlayed { get; set; }
            public string? SizeOnDisk { get; set; }
            public string? StagingSize { get; set; }
            [ValveKeyValue.KVProperty("buildid")]
            public string? BuildId { get; set; }
            public string? LastOwner { get; set; }
            public string? DownloadType { get; set; }
            public string? UpdateResult { get; set; }
            public string? BytesToDownload { get; set; }
            public string? BytesDownloaded { get; set; }
            public string? BytesToStage { get; set; }
            public string? BytesStaged { get; set; }
            public string? TargetBuildID { get; set; }
            public string? AutoUpdateBehavior { get; set; }
            public string? AllowOtherDownloadsWhileRunning { get; set; }
            public string? ScheduledAutoUpdate { get; set; }
            public Dictionary<string, CInstalledDepot>? InstalledDepots { get; set; }
            public CUserConfig? UserConfig { get; set; }
            public CMountedConfig? MountedConfig { get; set; }
        }

        public string File
        {
            get; set
            {
                if (field != value)
                {
                    field = value;
                    Invalidate();
                }
            }
        }
        public bool EnableInvalidate { get; set { field = value; if (!value) _invalide = false; } }
        public CAppState? AppState { get { if (_invalide) Validate(); return field; } set; }
        public string? GameInstalledDir
        {
            get
            {
                if (AppState?.Installdir is not string _v1)
                    return null;

                var _v2 = Path.GetDirectoryName(File);
                //if (string.IsNullOrEmpty(_v2))
                //    return null;
                return Path.Combine(_v2 ?? string.Empty, "common", _v1);
            }
        }
        private bool _invalide = false;

        public Acf(string filePath, bool allowInvalidate = true)
        {
            EnableInvalidate = allowInvalidate;
            File = filePath;
        }

        public void Invalidate()
        {
            if (!EnableInvalidate)
                return;
            _invalide = true;
        }

        public void Validate()
        {
            using var stream = System.IO.File.OpenRead(File);
            AppState = ValveKeyValue.KVSerializer.Create(ValveKeyValue.KVSerializationFormat.KeyValues1Text).Deserialize<CAppState>(stream);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    public static async Task<IReadOnlyList<Acf>> EnumInstalledGamesAsync()
    {
        if ((await SteamLocator.LocateSteamDirWindowsAsync())?.EndsWith(@":\Program Files (x86)\Steam")??false)
            return [.. (await Task.WhenAll(DriveInfo.GetDrives().Select(d => EnumInstalledGamesForDriverAsync(d.Name))))
                .SelectMany(games => games)];
        else
            return [.. await EnumInstalledGamesForSteamLibraryAsync(),
                .. (await Task.WhenAll(DriveInfo.GetDrives().Select(d => EnumInstalledGamesForDriverAsync(d.Name))))
                    .SelectMany(games => games)];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_steampath_override"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    public static async Task<IReadOnlyList<Acf>> EnumInstalledGamesForSteamLibraryAsync(string? _steampath_override = null)
    {
        var steamPath = _steampath_override ?? await SteamLocator.LocateSteamDirWindowsAsync();
        if (steamPath is not null)
            return await EnumGamesFromSteamLibraryAsyncInternal(steamPath);
        return [];
    }

    [SupportedOSPlatform("windows")]
    public static Task<IReadOnlyList<Acf>> EnumInstalledGamesForDriverAsync(string driver = @"C:\") => Task.Run(async () =>
    {
        var st_lib = Path.Combine(driver, "Program Files (x86)", "Steam");
        if (Directory.Exists(st_lib))
            return await EnumGamesFromSteamLibraryAsyncInternal(st_lib);
        st_lib = Path.Combine(driver, "SteamLibrary");
        if (Directory.Exists(st_lib))
            return await EnumGamesFromSteamLibraryAsyncInternal(st_lib);
        return [];
    });

    internal static Task<IReadOnlyList<Acf>> EnumGamesFromSteamLibraryAsyncInternal(string steamLibraryPath = @"C:\Program Files (x86)\Steam") => Task.Run(async () =>
    {
        List<Acf> list = [];

        string st_apps = Path.Combine(steamLibraryPath, "steamapps")
            //, st_apps_com = Path.Combine(st_apps,"common")
            ;
        if (!Directory.Exists(st_apps)
            //|| !Directory.Exists(st_apps_com)
            )
            goto end;

        foreach (var acfPath in Directory.EnumerateFiles(st_apps, "*.acf", SearchOption.TopDirectoryOnly))
        {
            list.Add(new Acf(acfPath));
        }

    end:
        return (IReadOnlyList<Acf>)list;
    });
}
