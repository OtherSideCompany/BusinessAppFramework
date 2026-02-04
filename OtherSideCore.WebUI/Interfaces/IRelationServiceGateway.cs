using OtherSideCore.Application.Relations;
using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtherSideCore.WebUI.Interfaces
{
    public interface IRelationServiceGateway
    {
        Task SetParentAsync(int parentId, int childId, string key);
    }
}
