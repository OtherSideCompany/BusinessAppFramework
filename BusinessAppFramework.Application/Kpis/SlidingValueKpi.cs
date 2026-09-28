using BusinessAppFramework.Contracts.Enums;

namespace BusinessAppFramework.Application.Kpis
{
    public class SlidingValueKpi
    {
        /// <summary>The timeframe as configured, e.g. 6 months; displayed on the card.</summary>
        public int TimeframeValue { get; set; }

        public TimeUnit TimeframeUnit { get; set; } = TimeUnit.Day;

        /// <summary>The timeframe converted to days, used to slide the periods.</summary>
        public int TimeframeDays { get; set; }

        public List<decimal> Values { get; set; } = new List<decimal>();

        public decimal CurrentValue => Values.Any() ? Values[0] : 0;

        public decimal ValueDelta(int index)
        {
            if (Values.Count() == 0)
                return 0;

            if (Values.Count() == 1)
                return Values[0];

            return Values[0] - Values[1];
        }

        public int PercentDelta(int index)
        {
            if (Values.Count() == 0 || Values.Count() == 1)
                return 0;

            return Values[index] != 0 ? (int)Math.Round(ValueDelta(index) / Values[index]) : 0;
        }

        public static Task<SlidingValueKpi> BuildAsync(int timeframeDays, Func<DateTime, DateTime, Task<decimal>> computePeriodValue, int periodsCount = 2)
        {
            return BuildAsync(timeframeDays, TimeUnit.Day, computePeriodValue, periodsCount);
        }

        public static async Task<SlidingValueKpi> BuildAsync(int timeframeValue, TimeUnit timeframeUnit, Func<DateTime, DateTime, Task<decimal>> computePeriodValue, int periodsCount = 2)
        {
            var now = DateTime.Now;
            var timeframeDays = timeframeUnit.ToDays(timeframeValue);

            var slidingValueKpi = new SlidingValueKpi
            {
                TimeframeValue = timeframeValue,
                TimeframeUnit = timeframeUnit,
                TimeframeDays = timeframeDays
            };

            for (var periodsAgo = 0; periodsAgo < periodsCount; periodsAgo++)
            {
                var periodStart = now.AddDays(-timeframeDays * (periodsAgo + 1));
                var periodEnd = now.AddDays(-timeframeDays * periodsAgo);

                slidingValueKpi.Values.Add(await computePeriodValue(periodStart, periodEnd));
            }

            return slidingValueKpi;
        }
    }
}
