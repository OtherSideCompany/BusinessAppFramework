namespace BusinessAppFramework.Contracts.Enums
{
    public static class TimeUnitExtensions
    {
        public static int ToDays(this TimeUnit unit, int value)
        {
            var now = DateTime.Now;

            var start = unit switch
            {
                TimeUnit.Day => now.AddDays(-value),
                TimeUnit.Month => now.AddMonths(-value),
                TimeUnit.Year => now.AddYears(-value),
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };

            return (now - start).Days;
        }
    }
}
