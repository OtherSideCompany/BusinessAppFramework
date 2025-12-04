using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Interfaces
{
    public interface IApplicationAction
    {
        StringKey ActionKey { get; }
        string ExecuteRouteTemplate { get; }
        HttpMethod HttpMethod { get; }
        int? DomainObjectId { get; }
        string BuildRoute();
    }
}
