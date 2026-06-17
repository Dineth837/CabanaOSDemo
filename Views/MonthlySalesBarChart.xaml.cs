using System;
using System.Windows.Controls;

namespace CabanaOSDemo.Views
{
    public partial class MonthlySalesBarChart : UserControl
    {
        private const double MaxPixelHeight = 150.0;
        private const double RevenueCeilingLimit = 500000.0;

        public MonthlySalesBarChart()
        {
            InitializeComponent();
            SetupBarInteractivity();
        }

        // 🚀 TEST MODE: Force the chart to show manual values for debugging
        public void SetManualValues(double[] restValues, double[] suiteValues)
        {
            Border[] restBars = { BarJanRest, BarFebRest, BarMarRest, BarAprRest, BarMayRest, BarJunRest,
                                  BarJulRest, BarAugRest, BarSepRest, BarOctRest, BarNovRest, BarDecRest };

            Border[] suiteBars = { BarJanSuite, BarFebSuite, BarMarSuite, BarAprSuite, BarMaySuite, BarJunSuite,
                                   BarJulSuite, BarAugSuite, BarSepSuite, BarOctSuite, BarNovSuite, BarDecSuite };

            for (int i = 0; i < 12; i++)
            {
                restBars[i].Height = CalculateRelativeHeight(restValues[i]);
                suiteBars[i].Height = CalculateRelativeHeight(suiteValues[i]);
            }
        }

        public void PopulateDynamicData(
            double janR, double janS, double febR, double febS, double marR, double marS,
            double aprR, double aprS, double mayR, double mayS, double junR, double junS,
            double julR, double julS, double augR, double augS, double sepR, double sepS,
            double octR, double octS, double novR, double novS, double decR, double decS)
        {
            BarJanRest.Height = CalculateRelativeHeight(janR); BarJanSuite.Height = CalculateRelativeHeight(janS);
            BarFebRest.Height = CalculateRelativeHeight(febR); BarFebSuite.Height = CalculateRelativeHeight(febS);
            BarMarRest.Height = CalculateRelativeHeight(marR); BarMarSuite.Height = CalculateRelativeHeight(marS);
            BarAprRest.Height = CalculateRelativeHeight(aprR); BarAprSuite.Height = CalculateRelativeHeight(aprS);
            BarMayRest.Height = CalculateRelativeHeight(mayR); BarMaySuite.Height = CalculateRelativeHeight(mayS);
            BarJunRest.Height = CalculateRelativeHeight(junR); BarJunSuite.Height = CalculateRelativeHeight(junS);
            BarJulRest.Height = CalculateRelativeHeight(julR); BarJulSuite.Height = CalculateRelativeHeight(julS);
            BarAugRest.Height = CalculateRelativeHeight(augR); BarAugSuite.Height = CalculateRelativeHeight(augS);
            BarSepRest.Height = CalculateRelativeHeight(sepR); BarSepSuite.Height = CalculateRelativeHeight(sepS);
            BarOctRest.Height = CalculateRelativeHeight(octR); BarOctSuite.Height = CalculateRelativeHeight(octS);
            BarNovRest.Height = CalculateRelativeHeight(novR); BarNovSuite.Height = CalculateRelativeHeight(novS);
            BarDecRest.Height = CalculateRelativeHeight(decR); BarDecSuite.Height = CalculateRelativeHeight(decS);
        }

        private double CalculateRelativeHeight(double rawRevenue)
        {
            // 1. Fixed constraints as requested
            const double MaxPixelHeight = 150.0;
            const double RevenueCeilingLimit = 500000.0; // The fixed cap

            // 2. Handle cases where revenue exceeds the cap (ensure it doesn't break UI)
            double effectiveRevenue = Math.Min(rawRevenue, RevenueCeilingLimit);

            // 3. Handle zero or negative revenue
            if (effectiveRevenue <= 0) return 0;

            // 4. Calculate relative height (0.0 to 1.0 ratio * max height)
            double calculatedHeight = (effectiveRevenue / RevenueCeilingLimit) * MaxPixelHeight;

            // 5. Ensure a minimum height for visibility if revenue > 0
            return Math.Max(5.0, calculatedHeight);
        }


        private void SetupBarInteractivity()
        {
            // List of all your bars
            var allBars = new[] {
            BarJanRest, BarJanSuite, BarFebRest, BarFebSuite,BarFebSuite, BarFebSuite, BarMarRest, BarMarSuite, 
            BarAprRest, BarAprSuite, BarMayRest, BarMaySuite, BarJunRest, BarJunSuite, BarJulRest, BarJulSuite, 
            BarAugRest, BarAugSuite, BarSepRest, BarSepSuite, BarOctRest, BarOctSuite, BarNovRest, BarNovSuite, 
            BarDecRest, BarDecSuite};

            foreach (var bar in allBars)
            {
                if (bar == null) continue;

                // 1. Add ToolTip
                string revenueType = bar.Name.Contains("Rest") ? "Restaurant" : "Suites";
                bar.ToolTip = $"{bar.Name.Substring(3, 3)} {revenueType} Revenue";

                // 2. Add Hover Effects
               
                //bar.MouseEnter += Bar_MouseEnter;
                //bar.MouseLeave += Bar_MouseLeave;
            }
        }


        private void Bar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var bar = (System.Windows.Controls.Border)sender;
            bar.Opacity = 0.7; // Lighten/Blur effect
            bar.Effect = new System.Windows.Media.Effects.DropShadowEffect { BlurRadius = 10, ShadowDepth = 0 };
        }

        private void Bar_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var bar = (System.Windows.Controls.Border)sender;
            bar.Opacity = 1.0;
            bar.Effect = null;
        }
    }
}