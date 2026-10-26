using System.Collections.Generic;

public enum Dictionaries
{
    Vivy,
    Skarnik,
    Verbum
}

public static class DictionaryLink
{
    private static readonly Dictionary<Dictionaries, string> _links = new Dictionary<Dictionaries, string>
    {
        [Dictionaries.Vivy] = "https://dictionaries.vivy.app/?q={word}",
        [Dictionaries.Skarnik] = "https://www.skarnik.by/search?term={word}&lang=beld",
        [Dictionaries.Verbum] = "https://verbum.by/?q={word}"
    };
    public static string GetSearchLink(Dictionaries dict, string word)
        => $" ({_links[dict].Replace("{word}", word)})";
}