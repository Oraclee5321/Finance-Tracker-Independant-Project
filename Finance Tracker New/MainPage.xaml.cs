using System.Windows.Input;

namespace Finance_Tracker_New;
using SkiaSharp;
using SkiaSharp.Views.Maui;

public partial class MainPage : ContentPage
{
    
    public string CurrentSpending { get; set; } = "£0";
    public string TotalBudget { get; set; } = "£600";
    
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
    

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);
        
        var data = new List<float> {25, 35, 40};
        var colors = new List<SKColor> {SKColors.Red, SKColors.Green, SKColors.Blue};

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
            startAngle += sweepAngle;
            
        }
    }
}