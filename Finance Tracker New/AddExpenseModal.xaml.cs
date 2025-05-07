using System;

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
        if (decimal.TryParse(ExpenseEntry.Text, out var amount))
        {
            ExpenseAdded?.Invoke(this, amount);
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Error", "Invalid amount entered", "OK");
        }
    }
}