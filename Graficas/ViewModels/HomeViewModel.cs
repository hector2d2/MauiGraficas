using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;

namespace Graficas.ViewModels
{
    [ObservableObject]
    public partial class HomeViewModel
	{
		[ObservableProperty]
        IEnumerable<ISeries> series;

        [ObservableProperty]
        LabelVisual title;

        // Datos de grafica de barra
        private readonly Random _r = new();
        private static readonly (string, double)[] s_initialData =
        {
            ("Tsunoda", 500),
            ("Sainz", 450),
            ("Riccardo", 520),
            ("Bottas", 550),
            ("Perez", 660),
            ("Verstapen", 920),
            ("Hamilton", 1000)
        };

        [ObservableProperty]
        ISeries[] seriesPieChar =
            s_initialData
                .Select(x => new RowSeries<ObservableValue>
                {
                    Values = new[] { new ObservableValue(x.Item2) },
                    Name = x.Item1,
                    Stroke = null,
                    MaxBarWidth = 25,
                    DataLabelsPaint = new SolidColorPaint(new SKColor(245, 245, 245)),
                    DataLabelsPosition = DataLabelsPosition.End,
                    DataLabelsTranslate = new LvcPoint(-1, 0),
                    DataLabelsFormatter = point => $"{point.Context.Series.Name} {point.PrimaryValue}"
                })
                .OrderByDescending(x => ((ObservableValue[])x.Values!)[0].Value)
                .ToArray();

        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) } };

        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false } };


        public HomeViewModel()
		{
            var data = new double[] { 2, 4, 1, 4, 3 };
            Series = data.AsLiveChartsPieSeries((value, series) =>
            {
                series.Name = $"Series for value {value}";
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
                series.DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
            });
            Title = new LabelVisual
            {
                Text = "My chart title",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            foreach (var item in SeriesPieChar)
            {
                if (item.Values is null) continue;

                var i = ((ObservableValue[])item.Values)[0];
                i.Value += _r.Next(0, 100);
            }

            //SeriesPieChar = Series.OrderByDescending(x => ((ObservableValue[])x.Values!)[0].Value).ToArray();

        }
    }
}