using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

public static class FileLoader
{
    public static async Task<string?> LoadAssFileAsync(string title, Window window)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(window);
            if (topLevel == null) return null;

            var assFileType = new FilePickerFileType("ASS/SSA Files")
            {
                Patterns = new[] { "*.ass", "*.ssa" }
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = new[] { assFileType }
            });

            if (files.Count == 0) return null;

            return await ReadAssFileAsync(files[0]);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
            return null;
        }
    }

    public static async Task<string?> ReadAssFileAsync(IStorageFile file)
    {
        try
        {
            await using var stream = await file.OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            return await streamReader.ReadToEndAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading {file.Name}: {ex.Message}");
            return null;
        }
    }

    public static async Task<string?> ReadAssFileAsync(string path)
    {
        try
        {
            return await File.ReadAllTextAsync(path);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading {path}: {ex.Message}");
            return null;
        }
    }
}
