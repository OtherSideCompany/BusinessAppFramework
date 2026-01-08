using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Interfaces
{
    public interface IDomainObjectApplicationAction
    {
        StringKey ActionKey { get; }
        string ExecuteRouteTemplate { get; }
        int? DomainObjectId { get; }
        string BuildRoute();
    }
}
