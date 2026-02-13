using BusinessAppFramework.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IDocumentNavigationApplicationAction : IDomainObjectApplicationAction
    {
        StringKey DocumentKey { get; }
    }
}
