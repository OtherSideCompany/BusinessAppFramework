namespace BusinessAppFramework.Application.Kpis
{
    public class SlidingValueKpi
    {
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
    }
}
