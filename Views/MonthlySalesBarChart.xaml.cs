using System;
using System.Windows.Controls;

namespace CabanaOSDemo.Views
{
    public partial class MonthlySalesBarChart : UserControl
    {
        private const double MaxPixelHeight = 150.0;
        private const double RevenueCeilingLimit = 100000.0;

        public MonthlySalesBarChart()
        {
            InitializeComponent();
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
            // TEMPORARY: Return 20 pixels so we can see if the bars actually render
            if (rawRevenue <= 0) return 0;

            double calculatedHeight = (rawRevenue / RevenueCeilingLimit) * MaxPixelHeight;
            return Math.Max(20.0, Math.Min(MaxPixelHeight, calculatedHeight)); // Clamp between 20 and 150
        }
    }
}