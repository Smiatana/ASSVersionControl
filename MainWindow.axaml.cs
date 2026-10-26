using AegisubVersionControl.Views.Settings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using System.IO;
using System.Linq;

namespace AegisubVersionControl;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _currentSettings = SettingsStore.Load();
        ChangesDisplay.SelectedDictionary = _currentSettings.DictionaryLink;
        DragDrop.SetAllowDrop(DropSurface, true);
        DragDrop.AddDragOverHandler(DropSurface, OnDragOver);
        DragDrop.AddDropHandler(DropSurface, OnDrop);
    }
    string? _translated;
    string? _edited;

    AppSettings _currentSettings = new AppSettings();

    private async void LoadTranslatedButton_Click(object sender, RoutedEventArgs e)
    {
        _translated = await FileLoader.LoadAssFileAsync("Load Translated ASS File", this);
        if (_translated != null)
        {
            TranslatedFileLabel.Text = "Файл загружаны";
        }
    }

    private async void LoadEditedButton_Click(object sender, RoutedEventArgs e)
    {
        _edited = await FileLoader.LoadAssFileAsync("Load Edited ASS File", this);
        if (_edited != null)
        {
            EditedFileLabel.Text = "Файл загружаны";
        }
    }

    private void FindChangesButton_Click(object sender, RoutedEventArgs e)
    {
        if (_translated != null && _edited != null)
        {
            var translatedLines = SubsParser.ParseSubtitleData(_translated);
            var editedLines = SubsParser.ParseSubtitleData(_edited);
            var changes = ChangesFinder.FindChanges(translatedLines, editedLines, _currentSettings);
            string changesReport = ChangesReporter.GetChangeReport(changes, _currentSettings);
            ChangesDisplay.Text = changesReport;
        }
        else
        {
            ChangesDisplay.Text = "Спачатку загрузіце абедзве версіі перакладу.";
        }
    }

    private async void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow();
        var saved = await settings.ShowDialog<bool>(this);
        if (!saved) return;

        _currentSettings = settings.SavedSettings;
        ChangesDisplay.SelectedDictionary = _currentSettings.DictionaryLink;
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        var paths = GetDroppedPaths(e.DataTransfer);
        var subtitleCount = paths.Count(IsSubtitleFile);
        var hasFilePayload = e.DataTransfer.Contains(DataFormat.File);
        e.DragEffects = subtitleCount > 0 || hasFilePayload
            ? DragDropEffects.Copy
            : DragDropEffects.None;
        DropStatusText.Text = subtitleCount > 0
            ? $"Адпусціце файлы, каб загрузіць: {subtitleCount}"
            : hasFilePayload
                ? "Атрыманы перанос файлаў; адпусціце для праверкі"
                : "Падтрымліваюцца файлы .ass і .ssa";
    }

    private async void OnDrop(object? sender, DragEventArgs e)
    {
        var paths = GetDroppedPaths(e.DataTransfer).Where(IsSubtitleFile).ToList();
        if (paths.Count == 0)
        {
            DropStatusText.Text = e.DataTransfer.Contains(DataFormat.File)
                ? "Файлы атрыманы, але іх шляхі не распазнаны."
                : "У перацягнутых даных няма файлаў. Паспрабуйце перацягнуць з файлавага мэнэджара.";
            return;
        }

        if (paths.Count >= 2)
        {
            _translated = await FileLoader.ReadAssFileAsync(paths[0]);
            _edited = await FileLoader.ReadAssFileAsync(paths[1]);
            if (_translated != null) TranslatedFileLabel.Text = Path.GetFileName(paths[0]);
            if (_edited != null) EditedFileLabel.Text = Path.GetFileName(paths[1]);
            DropStatusText.Text = _translated != null && _edited != null
                ? "Загружаны абодва файлы. Націсніце «Знайсці змены»."
                : "Не ўдалося прачытаць адзін або абодва файлы. Праверце доступ да іх.";
            return;
        }

        var path = paths[0];
        var content = await FileLoader.ReadAssFileAsync(path);
        if (content == null)
        {
            DropStatusText.Text = $"Не ўдалося прачытаць файл: {Path.GetFileName(path)}";
            return;
        }
        var name = Path.GetFileName(path);
        if (_translated == null)
        {
            _translated = content;
            TranslatedFileLabel.Text = name;
        }
        else
        {
            _edited = content;
            EditedFileLabel.Text = name;
        }
        DropStatusText.Text = $"Загружаны файл: {name}. Перацягніце другі файл для параўнання.";
    }

    private static bool IsSubtitleFile(string path) =>
        string.Equals(Path.GetExtension(path), ".ass", System.StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Path.GetExtension(path), ".ssa", System.StringComparison.OrdinalIgnoreCase);

    private static System.Collections.Generic.List<string> GetDroppedPaths(Avalonia.Input.IDataTransfer data)
    {
        var paths = data.TryGetFiles()?
            .Select(item => NormalizeDroppedPath(item.Path.LocalPath))
            .ToList() ?? new System.Collections.Generic.List<string>();
        return paths.Distinct(System.StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static string NormalizeDroppedPath(string path) =>
        System.Uri.TryCreate(path, System.UriKind.Absolute, out var uri) && uri.IsFile
            ? uri.LocalPath
            : path;

    private async void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(ChangesDisplay.Text);
        }
    }
}
