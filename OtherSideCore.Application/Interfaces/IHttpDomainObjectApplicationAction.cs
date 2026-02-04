using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Interfaces
{
    public interface IHttpDomainObjectApplicationAction : IDomainObjectApplicationAction
    {
        HttpMethod HttpMethod { get; }        
    }
}
