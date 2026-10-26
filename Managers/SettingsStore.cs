using System;
using System.IO;
using Newtonsoft.Json;

public static class SettingsStore
{
    public static string FilePath
    {
        get
        {
            if (OperatingSystem.IsLinux())
            {
                var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                var configHome = string.IsNullOrWhiteSpace(xdgConfigHome)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config")
                    : xdgConfigHome;
                return Path.Combine(configHome, "AegisubVersionControl", "AppSettings.json");
            }
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrWhiteSpace(basePath))
                basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");
            return Path.Combine(basePath, "AegisubVersionControl", "AppSettings.json");
        }
    }

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(FilePath))
                return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(FilePath)) ?? new AppSettings();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Could not read settings: {ex.Message}");
        }
        return new AppSettings();
    }

    public static void Save(AppSettings settings)
    {
        var directory = Path.GetDirectoryName(FilePath)!;
        Directory.CreateDirectory(directory);
        var temporaryPath = FilePath + ".tmp";
        File.WriteAllText(temporaryPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
        File.Move(temporaryPath, FilePath, overwrite: true);
    }
}
