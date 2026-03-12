using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Repository
{
    public interface IStatusRepository
    {
        Task SetStatusAsync(int domainObjectId, string statusKey);
        Task<string?> GetStatusAsync(int domainObjectId);

    }
}
