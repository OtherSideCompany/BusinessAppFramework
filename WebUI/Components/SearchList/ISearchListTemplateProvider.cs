using Application.Search;

namespace WebUI.Components.SearchList
{
   public interface ISearchListTemplateProvider<T> where T : DomainObjectSearchResult
   {
      SearchListTemplate<T>? Template { get; }
   }
}
