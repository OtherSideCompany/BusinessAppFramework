using System.Collections.Generic;
using System.Linq;

namespace OtherSideCore.Domain.DomainObjects
{
    public class DomainObjectReferenceList
    {
        #region Fields

        public string RelationKey { get; set; }
        public List<DomainObjectReferenceListItem> Items { get; set;  } = new();

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
                Items.Add(new DomainObjectReferenceListItem(domainObjectId, ""));
            }
        }

        #endregion

        #region Public Methods

        public void AddItem(int domainObjectId)
        {
            if (!Items.Any(i => i.DomainObjectId == domainObjectId))
            {
                Items.Add(new DomainObjectReferenceListItem(domainObjectId, ""));
            }
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
