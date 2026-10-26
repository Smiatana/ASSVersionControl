using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace AegisubVersionControl.Views.Settings;

public partial class SettingsWindow : Window
{
    private AppSettings _appSettings = new AppSettings();
    public AppSettings SavedSettings => _appSettings;

    public SettingsWindow()
    {
        AvaloniaXamlLoader.Load(this);

        ShowAddedCheckBox = this.FindControl<CheckBox>("ShowAddedCheckBox");
        ShowDeletedCheckBox = this.FindControl<CheckBox>("ShowDeletedCheckBox");
        ShowCommentsCheckBox = this.FindControl<CheckBox>("ShowCommentsCheckBox");
        ShowRowNumberCheckBox = this.FindControl<CheckBox>("ShowRowNumberCheckBox");
        ShowTimeCodesCheckBox = this.FindControl<CheckBox>("ShowTimeCodesCheckBox");
        UnderlineChangesCheckBox = this.FindControl<CheckBox>("UnderlineChangesCheckBox");
        SaveSettingsButton = this.FindControl<Button>("SaveSettingsButton");
        LoadDefaultsButton = this.FindControl<Button>("LoadDefaultsButton");
        DictionariesComboBox = this.FindControl<ComboBox>("DictionariesComboBox");

        if (DictionariesComboBox != null)
            DictionariesComboBox.ItemsSource = Enum.GetValues(typeof(Dictionaries));

        _appSettings = SettingsStore.Load();
        PutSettings();
    }

    private void PutSettings()
    {
        try
        {
            if (ShowAddedCheckBox != null)
                ShowAddedCheckBox.IsChecked = _appSettings.ShowAdded;

            if (ShowDeletedCheckBox != null)
                ShowDeletedCheckBox.IsChecked = _appSettings.ShowDeleted;

            if (ShowCommentsCheckBox != null)
                ShowCommentsCheckBox.IsChecked = _appSettings.ShowComments;

            if (ShowRowNumberCheckBox != null)
                ShowRowNumberCheckBox.IsChecked = _appSettings.ShowIndexes;

            if (ShowTimeCodesCheckBox != null)
                ShowTimeCodesCheckBox.IsChecked = _appSettings.ShowTimeCodes;

            if (UnderlineChangesCheckBox != null)
                UnderlineChangesCheckBox.IsChecked = _appSettings.UnderlineChanges;

            if (DictionariesComboBox != null)
                DictionariesComboBox.SelectedItem = _appSettings.DictionaryLink;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying settings to UI: {ex}");
        }
    }


    private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _appSettings = new AppSettings
            {
                ShowAdded = ShowAddedCheckBox?.IsChecked ?? false,
                ShowDeleted = ShowDeletedCheckBox?.IsChecked ?? false,
                ShowComments = ShowCommentsCheckBox?.IsChecked ?? true,
                ShowIndexes = ShowRowNumberCheckBox?.IsChecked ?? true,
                ShowTimeCodes = ShowTimeCodesCheckBox?.IsChecked ?? false,
                UnderlineChanges = UnderlineChangesCheckBox?.IsChecked ?? true,
                DictionaryLink = (Dictionaries)(DictionariesComboBox?.SelectedItem ?? Dictionaries.Vivy)
            };

            SettingsStore.Save(_appSettings);
            Close(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving settings: {ex}");
        }
    }

    private void LoadDefaultsButton_Click(object sender, RoutedEventArgs e)
    {
        _appSettings = new AppSettings();
        PutSettings();
    }
}
