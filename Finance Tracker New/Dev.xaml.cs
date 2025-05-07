using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Finance_Tracker_New;

public partial class Dev : ContentPage
{
    private MainPage _mainPage;
    
    public Dev(MainPage mainPage)
    {
        InitializeComponent();
        _mainPage = mainPage;
    }
    private void OnChangeMoneyClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(MoneyEntry.Text, out var newAmount))
        {
            _mainPage.CurrentSpending = $"£{newAmount}";
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
            _mainPage.TotalBudget = $"£{newBudget}";
            DisplayAlert("Success", $"Total budget updated to £{newBudget}", "OK");
        }
        else
        {
            DisplayAlert("Error", "Invalid budget entered", "OK");
        }
    }
    
}