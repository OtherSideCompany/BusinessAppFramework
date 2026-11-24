using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessAppFramework.WebUI.Helpers
{
    public static class YearRangeHelper
    {
        public static List<int> YearsSince(int firstYear)
        {
            var currentYear = DateTime.Now.Year;

            if (firstYear < 1 || firstYear > currentYear)
            {
                firstYear = currentYear;
            }

            return Enumerable.Range(firstYear, currentYear - firstYear + 1).ToList();
        }
    }
}
