using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Finance_Tracker_New;

public partial class Dev : ContentPage
{
    public Dev()
    {
        InitializeComponent();
    }
    
    private void OnChangeMoneyClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(MoneyEntry.Text, out var newAmount))
        {
            var spendingref = (Application.Current.MainPage.BindingContext as MainPage)?.CurrentSpending;
            spendingref = $"£{newAmount}";
            DisplayAlert("Success", $"Current spending updated to £{newAmount}", "OK");
        }
        else
        {
            DisplayAlert("Error", "Invalid amount entered", "OK");
        }
    }
    private void OnChangeBudgetClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(BudgetEntry.Text, out var newBudget))
        {
            if (Application.Current.MainPage.BindingContext is MainPage mainPage)
            {
                mainPage.TotalBudget = $"£{newBudget}";
            }
            DisplayAlert("Success", $"Total budget updated to £{newBudget}", "OK");
        }
        else
        {
            DisplayAlert("Error", "Invalid amount entered", "OK");
        }
    }
}