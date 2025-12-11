using OtherSideCore.Domain;
using OtherSideCore.Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.Application.Interfaces
{
    public interface IReferenceHydrator
    {
        Task HydrateAsync(DomainObject domainObject);
    }
}
