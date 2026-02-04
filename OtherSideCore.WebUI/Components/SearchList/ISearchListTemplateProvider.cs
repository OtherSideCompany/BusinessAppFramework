using OtherSideCore.Application.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Components.SearchList
{
    public interface ISearchListTemplateProvider<T> where T : DomainObjectSearchResult
    {
        SearchListTemplate<T>? Template { get; }
    }
}
