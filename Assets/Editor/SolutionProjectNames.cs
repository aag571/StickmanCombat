using System.Text.RegularExpressions;
using UnityEditor;

public class SolutionProjectNames : AssetPostprocessor
{
    public static string OnGeneratedSlnSolution(string path, string content)
    {
        return Regex.Replace(content,
            @"^(Project\([^\r\n]+?\) = "")([^""\r\n]+)("", ""[^""\r\n]+\.Player\.csproj"")",
            match => match.Groups[1].Value
                + (match.Groups[2].Value.EndsWith(".Player") ? match.Groups[2].Value : match.Groups[2].Value + ".Player")
                + match.Groups[3].Value,
            RegexOptions.Multiline);
    }
}
