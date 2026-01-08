using OtherSideCore.Domain.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace OtherSideCore.Domain.DomainObjects
{
    public abstract class DomainObject : IDisposable, ICloneable
    {
        #region Fields



        #endregion

        #region Properties

        [SystemProperty]
        public int Id { get; set; }

        [SystemProperty]
        public DateTime CreationDate { get; set; }

        [SystemProperty]
        public int? CreatedById { get; set; }

        [SystemProperty]
        public string? CreatedByName { get; set; }

        [SystemProperty]
        public DateTime LastModifiedDateTime { get; set; }

        [SystemProperty]
        public int? LastModifiedById { get; set; }

        [SystemProperty]
        public string? LastModifiedByName { get; set; }

        #endregion

        #region Constructor

        public DomainObject()
        {

        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            var item = obj as DomainObject;

            if (item == null)
            {
                return false;
            }

            if (Id == 0 && item.Id == 0)
            {
                return GetHashCode() == item.GetHashCode();
            }
            else
            {
                return Id == item.Id;
            }
        }

        public virtual object Clone()
        {
            var domainObject = (DomainObject)MemberwiseClone();

            domainObject.Id = 0;
            domainObject.CreatedById = null;
            domainObject.LastModifiedById = null;
            domainObject.CreatedByName = null;
            domainObject.LastModifiedByName = null;
            domainObject.CreationDate = DateTime.Now;
            domainObject.LastModifiedDateTime = DateTime.Now;

            return domainObject;
        }

        public IEnumerable<DomainObjectReference> GetReferences()
        {
            var props = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(DomainObjectReference).IsAssignableFrom(p.PropertyType));

            foreach (var prop in props)
            {
                yield return prop.GetValue(this) as DomainObjectReference;
            }
        }

        public IEnumerable<DomainObjectReferenceList> GetReferenceLists()
        {
            var props = GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => typeof(DomainObjectReferenceList).IsAssignableFrom(p.PropertyType));

            foreach (var prop in props)
            {
                yield return prop.GetValue(this) as DomainObjectReferenceList;
            }
        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Private Methods      


        #endregion
    }
}
