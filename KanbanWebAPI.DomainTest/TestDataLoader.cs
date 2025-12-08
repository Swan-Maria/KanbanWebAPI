using System.Text.Json;

namespace KanbanWebAPI.DomainTest;

public static class TestDataLoader
{
    public static List<T> LoadFromJson<T>(string fileName) where T : class
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", fileName);

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"JSON data file '{jsonFilePath}' not found.");
        }

        var jsonString = File.ReadAllText(jsonFilePath);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<List<T>>(jsonString, options) ?? new List<T>();
    }
}