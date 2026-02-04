namespace BusinessAppFramework.Application.Search
{
   public class PaginatedSearchRequest : SearchRequest
   {
      public int PageIndex { get; set; }
      public int PageSize { get; set; }
   }
}
