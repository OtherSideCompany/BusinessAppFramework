using BusinessAppFramework.Application.Search;

namespace BusinessAppFramework.WebUI.Components.SearchList
{
   public interface ISearchListTemplateProvider<T> where T : DomainObjectSearchResult
   {
      SearchListTemplate<T>? Template { get; }
   }
}
