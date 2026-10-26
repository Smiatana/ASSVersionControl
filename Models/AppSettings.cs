public class AppSettings
{
    public bool ShowAdded { get; set; }
    public bool ShowDeleted { get; set; }
    public bool ShowComments { get; set; } = true;
    public bool ShowIndexes { get; set; }
    public bool ShowTimeCodes { get; set; } = true;
    public bool UnderlineChanges { get; set; } = true;

    public Dictionaries DictionaryLink { get; set; } = Dictionaries.Verbum;
}
