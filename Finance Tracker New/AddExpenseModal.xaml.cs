﻿using System;
using System.Text.Json;

namespace Finance_Tracker_New;
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
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
                if (!File.Exists(filePath))
                {
                    using (var stream = File.Create(filePath))
                    {
                        // Create the file
                    }
                }
                var existingContent = await File.ReadAllTextAsync(filePath);
                // Count last id value
                int lastvalue;
                if (string.IsNullOrWhiteSpace(existingContent) || existingContent == "[]")
                {
                    lastvalue = 0;
                }
                else
                {
                    var lastExpense = JsonSerializer.Deserialize<List<Expense>>(existingContent)?.LastOrDefault();
                    lastvalue = lastExpense?.ID ?? 0;
                }
                var expense = new Expense
                {
                    ID = lastvalue + 1,
                    Date = DateTime.Now,
                    Name = expenseName,
                    Cost = amount
                };
                expense.addToJson();
                
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