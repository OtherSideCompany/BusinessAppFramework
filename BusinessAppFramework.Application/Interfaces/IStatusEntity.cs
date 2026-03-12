using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IStatusEntity : IEntity
    {
        public string StatusKey { get; set; }
    }
}
