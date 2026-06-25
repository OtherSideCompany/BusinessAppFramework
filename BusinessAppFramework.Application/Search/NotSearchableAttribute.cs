using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Search
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotSearchableAttribute : Attribute
    {
    }
}
