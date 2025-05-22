using System.Text.Json;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.Maui.Storage.FileSystem;

namespace Finance_Tracker_New;

public partial class ViewExpenseModal : ContentPage
{
    public event EventHandler<decimal> ExpenseRemoved;
    
    public List<Expense> Expenses { get; set; }
    

    public ViewExpenseModal()
    {
        InitializeComponent();
        Expenses = LoadExpenses() ?? new List<Expense>();
        BindingContext = this;
    }
    
    
    
    private void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var expense = (Expense)button.BindingContext;
        if (expense != null)
        {
            try
            {
                var delitem = Expenses.FirstOrDefault(x => x.ID == expense.ID);
                if (delitem != null)
                {
                    Expenses.Remove(delitem);
                    // Update the UI
                    BindingContext = null;
                    BindingContext = this;
                }

                // Update MainPage
                ExpenseRemoved?.Invoke(this, expense.Cost * -1);

                //remove from json
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var expenses = JsonSerializer.Deserialize<List<Expense>>(json);
                    if (expenses != null)
                    {
                        delitem = expenses.FirstOrDefault(x => x.ID == expense.ID);
                        if (delitem != null)
                        {
                            expenses.Remove(delitem);
                        }

                        json = JsonSerializer.Serialize(expenses);
                        File.WriteAllText(filePath, json);
                    }
                }

                // Show a message
                DisplayAlert("Success", "Expense deleted successfully.", "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to delete expense: {ex.Message}", "OK");
            }
        }
    }

    private List<Expense> LoadExpenses()
    {
        try
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Expense>>(json);
        }
        catch
        {
            return null;
        }
    }
    public void OnCloseClicked(object sender, EventArgs e)
    {  
        Navigation.PopModalAsync();
        
    }
}
