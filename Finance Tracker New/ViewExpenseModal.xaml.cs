using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Finance_Tracker_New;

public class displayExpense
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
}

public partial class ViewExpenseModal : ContentPage
{
    public List<displayExpense> LoadExpenses()
    {
        try
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
            if (!File.Exists(filePath))
            {
                DisplayAlert("Error", "No transactions found.", "OK");
                return null;
            }
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var expenses = JsonSerializer.Deserialize<List<displayExpense>>(json);
            if (expenses == null || !expenses.Any())
            {
                DisplayAlert("Error", "No transactions found.", "OK");
                return null;
            }
            return expenses;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Failed to load expenses: {ex.Message}", "OK");
            return null;
        }
    }

    public ViewExpenseModal()
    {
        InitializeComponent();
        // Load the expenses from transactions.json
        var data = LoadExpenses();
        var expenseList = new ListView();
        expenseList.ItemsSource = data;
        expenseList.ItemTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label();
            nameLabel.SetBinding(Label.TextProperty, "Name");

            var costLabel = new Label();
            costLabel.SetBinding(Label.TextProperty, "Cost");

            var dateLabel = new Label();
            dateLabel.SetBinding(Label.TextProperty, "Date");

            return new ViewCell
            {
                View = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        nameLabel,
                        costLabel,
                        dateLabel
                    }
                }
            };
        });
        Content = new StackLayout
        {
            Children =
            {
                new Label { Text = "Expenses" },
                expenseList
            }
        };
    }
}