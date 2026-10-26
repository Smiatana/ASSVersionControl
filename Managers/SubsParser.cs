using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public static class SubsParser
{
    public static List<SubtitleEntry> ParseSubtitleData(string fileContent)
    {
        var entries = new List<SubtitleEntry>();
        if (fileContent == null) return entries;

        string[] lines = fileContent.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        bool isSubsSection = false;

        foreach (var line in lines)
        {
            if (line.Trim().Equals("[Events]", System.StringComparison.OrdinalIgnoreCase))
            {
                isSubsSection = true;
                continue;
            }
            if (isSubsSection && line.StartsWith("Dialogue:", System.StringComparison.OrdinalIgnoreCase))
            {
                var entry = ParseDialogueLine(line);
                if (entry != null)
                    entries.Add(entry);
            }
        }
        return entries;
    }
    public static SubtitleEntry? ParseDialogueLine(string line)
    {
        try
        {
            string[] parts = line.Split(new[] { ',' }, 10);
            if (parts.Length < 10) return null;

            string startTime = parts[1].Trim();
            string endTime = parts[2].Trim();

            string textWithComment = parts[9].Trim();

            string? comment = null;
            string? text = null;

            if (textWithComment.Length > 0)
            {
                if (textWithComment.Contains('}'))
                {
                    var commentBuilder = new StringBuilder();
                    var matches = Regex.Matches(textWithComment, @"\{.*?\}");
                    foreach (Match match in matches)
                    {
                        commentBuilder.Append(match.Value);
                    }
                    comment = commentBuilder.Length > 0 ? commentBuilder.ToString() : null;
                    comment = comment?.Replace("{", "").Replace("}", "");
                }

                text = Regex.Replace(textWithComment, @"\{.*?\}", "").Trim();
            }
            return new SubtitleEntry
            {
                StartTime = startTime,
                EndTime = endTime,
                Content = text,
                Comment = comment
            };

        }
        catch (System.Exception)
        {
            return null;
        }
    }
}
