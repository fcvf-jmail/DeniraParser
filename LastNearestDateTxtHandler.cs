namespace DeniraParser;
public static class LastNearestDateTxtHandler
{
    private static readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "lastNearestDate.txt");

    public static string Read()
    {
        if (!File.Exists(_filePath)) File.Create(_filePath);
        return File.ReadAllText(_filePath);
    }

    // Rewrites the file with the provided content
    public static void Rewrite(string content)
    {
        if (!File.Exists(_filePath)) File.Create(_filePath);
        File.WriteAllText(_filePath, content);
    }
}
