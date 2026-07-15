using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IPictureProviderServiceGateway
    {
        Task<byte[]> GetPictureAsync(int domainObjectId);
    }
}
