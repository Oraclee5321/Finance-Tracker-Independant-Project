using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Finance_Tracker_New;
using SkiaSharp;
using SkiaSharp.Views.Maui;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    
    private string _currentSpending = "£0";
    
    private string _totalBudget = "£10000";
    
    public string CurrentSpending
    {
        get => _currentSpending;
        set
        {
            if (_currentSpending != value)
            {
                _currentSpending = value;
                OnPropertyChanged();
                PieChartCanvas.InvalidateSurface();
            }
        }
    }
    
    public string TotalBudget
    {
        get => _totalBudget;
        set
        {
            if (_totalBudget != value)
            {
                _totalBudget = value;
                OnPropertyChanged();
                PieChartCanvas.InvalidateSurface();
            }
        }
    }

    public void LoadSpending()
    {
        try
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
            if (!File.Exists(filePath))
            {
                DisplayAlert("Error", "No transactions found.", "OK");
                return;
            }
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var expenses = JsonSerializer.Deserialize<List<Expense>>(json);
            if (expenses == null || !expenses.Any())
            {
                DisplayAlert("Error", "No transactions found.", "OK");
                return;
            }
            CurrentSpending = $"£{expenses.Sum(e => e.Cost)}";
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Failed to load expenses: {ex.Message}", "OK");
        }
    }
    
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadSpending();
    }

    async private void OnAddExpenseClicked(object sender, EventArgs e)
    {
        var modal = new AddExpenseModalPage();
        modal.ExpenseAdded += (s, amount) =>
        {
            CurrentSpending = $"£{decimal.Parse(CurrentSpending.Trim('£')) + amount}";
            DisplayAlert("Success", $"Expense of £{amount} added.", "OK");
            PieChartCanvas.InvalidateSurface();
        };
        await Navigation.PushModalAsync(modal);
    }
    
    async private void OnViewExpenseClicked(object sender, EventArgs e)
    {
        var modal = new ViewExpenseModal();
        modal.ExpenseRemoved += (s, amount) =>
        {
            CurrentSpending = $"£{decimal.Parse(CurrentSpending.Trim('£')) + amount}";
            PieChartCanvas.InvalidateSurface();
        };
        await Navigation.PushModalAsync(modal);
    }


    private void PiechartDraw(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);
        
        var spending = decimal.Parse(CurrentSpending.Trim('£'));
        var budget = decimal.Parse(TotalBudget.Trim('£'));
        
        var data = new List<float> {
            (float)spending,
            (float)(budget - spending)
        };
        var colors = new List<SKColor> {SKColors.Red, SKColors.Green};
        var labels = new List<string> {"Spending", "Remaining Budget"};

        float startAngle = 0;
        var center = new SKPoint(e.Info.Width / 2, e.Info.Height / 2);
        float radius = Math.Min(e.Info.Width, e.Info.Height) / 2;

        for (int i = 0; i < data.Count; i++)
        {
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = colors[i],
            };
            
            float sweepAngle = 360 * (data[i] / data.Sum());
            var path = new SKPath();
            path.MoveTo(center);
            path.ArcTo(new SKRect(center.X - radius, center.Y -  radius, center.X + radius, center.Y + radius),startAngle, sweepAngle, false);
            path.Close();
            canvas.DrawPath(path, paint);

            var labelAngle = startAngle + sweepAngle / 2;
            var labelRadius = radius * 0.7f;
            var labelX = center.X + labelRadius * (float)Math.Cos(labelAngle * Math.PI / 180);
            var labelY = center.Y + labelRadius * (float)Math.Sin(labelAngle * Math.PI / 180);
            using var textPaint = new SKPaint
            {
                Color = SKColors.White,
                TextSize = 40,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };
            canvas.DrawText(labels[i], labelX, labelY, textPaint);
            startAngle += sweepAngle;
            
        }
    }
}