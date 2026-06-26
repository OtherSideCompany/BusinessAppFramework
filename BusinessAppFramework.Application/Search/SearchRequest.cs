namespace BusinessAppFramework.Application.Search
{
    public class SearchRequest
    {
        public int? SourceId { get; set; }
        public string SourceRelationKey { get; set; } = String.Empty;
        public bool ExtendedSearch { get; set; }
        public List<string> Filters { get; set; } = [];
        public string ConstraintKey { get; set; } = Contracts.ConstraintKeys.AllConstraintKey;
    }
}
