using System;

namespace BusinessAppFramework.WebUI.Helpers
{
    public static class ChartAxisHelper
    {
        public static int ComputeYAxisStep(decimal maxValue)
        {
            if (maxValue < 4)
            {
                return 1;
            }

            var rawStep = (double)maxValue / 4;
            var magnitude = Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
            var residual = rawStep / magnitude;
            var niceResidual = residual <= 1 ? 1 : residual <= 2 ? 2 : residual <= 5 ? 5 : 10;

            return (int)(niceResidual * magnitude);
        }
    }
}
