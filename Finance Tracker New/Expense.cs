using System.Text.Json;

namespace Finance_Tracker_New;

public class Expense
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }

    async public void addToJson()
    {
        string jsonExpense = JsonSerializer.Serialize(this);

        var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
        if (!File.Exists(filePath))
        {
            using (var stream = File.Create(filePath))
            {
                // Create the file
            }
        }
        var existingContent = await File.ReadAllTextAsync(filePath);
        if (string.IsNullOrWhiteSpace(existingContent) || existingContent == "[]")
        {
            existingContent = "[";
        }
        else
        {
            existingContent = existingContent.TrimEnd(']', '\n', '\r') + ",";
        }
        existingContent += jsonExpense + "\n]";
        await File.WriteAllTextAsync(filePath, existingContent);
    }
}