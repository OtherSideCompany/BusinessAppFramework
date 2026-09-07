namespace BusinessAppFramework.Application.Kpis
{
    public class SlidingValueKpi
    {
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

        public static async Task<SlidingValueKpi> BuildAsync(int timeframeDays, Func<DateTime, DateTime, Task<decimal>> computePeriodValue, int periodsCount = 2)
        {
            var now = DateTime.Now;
            var slidingValueKpi = new SlidingValueKpi { TimeframeDays = timeframeDays };

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
