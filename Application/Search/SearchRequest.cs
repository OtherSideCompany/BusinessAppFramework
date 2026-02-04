namespace Application.Search
{
   public class SearchRequest
   {
      public bool ExtendedSearch { get; set; }
      public List<string> Filters { get; set; } = [];
      public string ConstraintKey { get; set; } = Contracts.ConstraintKeys.AllConstraintKey;
   }
}
