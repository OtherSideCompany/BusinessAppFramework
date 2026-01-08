using OtherSideCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OtherSideCore.Application.Relations
{
    public interface IReferenceListRelationEntry
    {
        StringKey RelationKey { get; }
        Type SourceDomainObjectType { get; }
        Type TargetDomainObjectType { get; }
        Type SourceEntityType { get; }
        Type TargetEntityType { get; }
        PropertyInfo DomainProperty { get; }
        PropertyInfo EntityProperty { get; }
    }
}
