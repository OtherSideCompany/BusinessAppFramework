using System.Collections.Generic;
using System.Linq;

namespace OtherSideCore.Domain.DomainObjects
{
    public class DomainObjectReferenceList
    {
        #region Fields

        public string RelationKey { get; set; }
        public List<DomainObjectReference> DomainObjectReferences { get; set; } = new();

        #endregion

        #region Properties



        #endregion

        #region Events



        #endregion

        #region Constructor

        public DomainObjectReferenceList()
        {

        }

        public DomainObjectReferenceList(string relationKey, List<int> domainObjectIds)
        {
            RelationKey = relationKey;

            foreach (var domainObjectId in domainObjectIds)
            {
                DomainObjectReferences.Add(new DomainObjectReference(relationKey, domainObjectId));
            }
        }

        #endregion

        #region Public Methods

        public void AddReference(int domainObjectId)
        {
            if (!DomainObjectReferences.Any(r => r.DomainObjectId == domainObjectId))
            {
                DomainObjectReferences.Add(new DomainObjectReference(RelationKey, domainObjectId));
            }
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
