using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CabanaOSDemo.Views
{
    public partial class CabanaSuitesSalesPieChart : UserControl
    {
        public CabanaSuitesSalesPieChart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the percentage labels and redraws the vector paths dynamically based on file data.
        /// </summary>
        public void UpdateData(double stdCouple, double stdFamily, double superior, double deluxe)
        {
            double total = stdCouple + stdFamily + superior + deluxe;

            // Update Text Overlays (Percentages) Safely
            var textBlocks = FindVisualChildren<TextBlock>(this).Where(tb => tb.Text.Contains("%")).ToArray();
            if (textBlocks.Length >= 4)
            {
                textBlocks[0].Text = total > 0 ? $"{(stdCouple / total * 100):0}%" : "0%";
                textBlocks[1].Text = total > 0 ? $"{(stdFamily / total * 100):0}%" : "0%";
                textBlocks[2].Text = total > 0 ? $"{(superior / total * 100):0}%" : "0%";
                textBlocks[3].Text = total > 0 ? $"{(deluxe / total * 100):0}%" : "0%";
            }

            // Fallback safety shield to prevent empty chart calculations
            if (total == 0) { stdCouple = 1; stdFamily = 1; superior = 1; deluxe = 1; total = 4; }

            double[] values = { stdCouple, stdFamily, superior, deluxe };
            var chartPaths = FindVisualChildren<Path>(this).Where(p => p.Fill is SolidColorBrush).ToArray();

            // Redraw the actual geometric slices cleanly
            DrawPieChart(chartPaths, values, total);
        }

        private void DrawPieChart(Path[] paths, double[] values, double total)
        {
            if (paths == null || paths.Length == 0) return;

            double startAngle = 0;
            double radius = 80;
            Point center = new Point(100, 100); // Absolute center coordinates of your canvas

            for (int i = 0; i < Math.Min(paths.Length, values.Length); i++)
            {
                double share = values[i] / total;
                if (share >= 1.0) share = 0.9999; // Prevents full-circle mathematical drawing errors

                double endAngle = startAngle + (share * 360);

                double startRad = (Math.PI / 180) * (startAngle - 90);
                double endRad = (Math.PI / 180) * (endAngle - 90);

                Point p1 = new Point(center.X + radius * Math.Cos(startRad), center.Y + radius * Math.Sin(startRad));
                Point p2 = new Point(center.X + radius * Math.Cos(endRad), center.Y + radius * Math.Sin(endRad));

                bool isLargeArc = (endAngle - startAngle) > 180;

                // Construct a brand new, valid WPF PathGeometry block to prevent shape loss
                PathGeometry geom = new PathGeometry();
                PathFigure fig = new PathFigure { StartPoint = center, IsClosed = true };

                fig.Segments.Add(new LineSegment(p1, true));
                fig.Segments.Add(new ArcSegment(p2, new Size(radius, radius), 0, isLargeArc, SweepDirection.Clockwise, true));

                geom.Figures.Add(fig);
                paths[i].Data = geom; // Overwrites the old hardcoded XAML coordinates with dynamic data numbers

                startAngle = endAngle;
            }
        }

        // 🚀 FIXED: Appended '?' to parameter to clear null reference analysis warnings entirely
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) yield return (T)child;
                    foreach (T childOfChild in FindVisualChildren<T>(child)) yield return childOfChild;
                }
            }
        }
    }
}