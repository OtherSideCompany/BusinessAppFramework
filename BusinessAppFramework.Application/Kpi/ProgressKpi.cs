namespace BusinessAppFramework.Application.Kpi
{
    public class ProgressKpi : Kpi
    {         
        public decimal Done { get; set; }
        public decimal Total { get; set; }
        public decimal Percentage => Total == 0 ? 0 : Math.Round(Done / Total * 100, 0);
        public bool IsComplete => Total > 0 && Done >= Total;
    }
}
