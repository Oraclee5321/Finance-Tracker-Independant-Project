using System;
using System.Text.Json;

namespace Finance_Tracker_New;

public class Expense
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
}

public partial class AddExpenseModalPage : ContentPage
{
    public event EventHandler<decimal> ExpenseAdded;

    public AddExpenseModalPage()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var expenseName = ExpenseNameEntry.Text;
        if (decimal.TryParse(ExpenseEntry.Text, out var amount))
        {
            ExpenseAdded?.Invoke(this, amount);
            // Save the expense to transactions.json
            try
            {
                var expense = new Expense
                {
                    Date = DateTime.Now,
                    Name = expenseName,
                    Cost = amount
                };
                string jsonExpense = JsonSerializer.Serialize(expense);

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
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save expense: {ex.Message}", "OK");
            }
            
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Invalid amount entered", "OK");
        }
    }
}