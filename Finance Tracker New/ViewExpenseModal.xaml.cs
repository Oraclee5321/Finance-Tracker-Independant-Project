using System.Text.Json;

namespace Finance_Tracker_New;

public partial class ViewExpenseModal : ContentPage
{
    public List<displayExpense> Expenses { get; set; }
    

    public ViewExpenseModal()
    {
        InitializeComponent();
        Expenses = LoadExpenses() ?? new List<displayExpense>();
        BindingContext = this;
    }

    private List<displayExpense> LoadExpenses()
    {
        try
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<displayExpense>>(json);
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

public class displayExpense
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
}