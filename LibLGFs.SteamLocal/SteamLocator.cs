using Microsoft.Win32;
using System.Runtime.Versioning;

namespace LibLGFs.SteamLocal;

[SupportedOSPlatform("windows")]
public static class SteamLocator
{
    /// <summary>
    /// Locates the Steam directory on a Windows system
    /// </summary>
    /// <returns>The path to the Steam directory, null for not found</returns>
    public static async Task<string?> LocateSteamDirWindowsAsync()
    {
        // first, try to check C:\Program Files (x86)\Steam
        string commonPath = @"C:\Program Files (x86)\Steam";

        bool isCommonPath = await Task.Run(() => Directory.Exists(commonPath));
        if (isCommonPath)
        {
            return commonPath;
        }

        // if not found, try to look for it in windows registry
        string? foundInRegistry = await LocateSteamDirWindowsUsingRegistryAsync();
        //if (string.IsNullOrEmpty(foundInRegistry))
        //{
        //    throw new DirectoryNotFoundException("Steam directory not found");
        //}

        return foundInRegistry;
    }

    /// <summary>
    /// Locates the Steam directory on a Windows system using the registry
    /// </summary>
    /// <returns>The path to the Steam directory, or null if not found</returns>
    public static async Task<string?> LocateSteamDirWindowsUsingRegistryAsync()
    {
        return await Task.Run(() =>
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return FindInRegistry(@"HKLM\SOFTWARE\WOW6432Node\Valve\Steam");
            }

            return FindInRegistry(@"HKLM\SOFTWARE\Valve\Steam");
        });
    }

    /// <summary>
    /// Finds a value in the Windows registry
    /// </summary>
    /// <param name="registryPath">The registry path in format HIVE\KEY\SUBKEY</param>
    /// <returns>The value found, or null if not found</returns>
    private static string? FindInRegistry(string registryPath)
    {
        try
        {
            string[] parts = registryPath.Split('\\');
            if (parts.Length < 2)
                return null;

            string hive = parts[0];
            string keyPath = string.Join("\\", parts.Skip(1));

            RegistryKey? baseKey = hive switch
            {
                "HKLM" => Registry.LocalMachine,
                "HKCU" => Registry.CurrentUser,
                "HKCR" => Registry.ClassesRoot,
                "HKU" => Registry.Users,
                "HKCC" => Registry.CurrentConfig,
                _ => null
            };

            if (baseKey == null)
                return null;

            using RegistryKey? key = baseKey.OpenSubKey(keyPath);
            return key?.GetValue("InstallPath")?.ToString();
        }
        catch
        {
            return null;
        }
    }
}