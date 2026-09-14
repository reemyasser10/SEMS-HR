using System.Text;

namespace Utilities.FileHandler
{
    public static class FileUtil
    {
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            StringBuilder stringBuilder = new();
            using FileStream fileStream = File.OpenRead(filePath);
            using StreamReader streamReader = new(fileStream);
            string? line = await streamReader.ReadLineAsync();
            while (line != null)
            {
                _ = stringBuilder.AppendLine(line);
                line = await streamReader.ReadLineAsync();
            }
            return stringBuilder.ToString();
        }
    }
}
