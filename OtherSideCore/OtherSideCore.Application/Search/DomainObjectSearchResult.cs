using OtherSideCore.Application.Trees;
using OtherSideCore.Domain.DomainObjects;

namespace OtherSideCore.Application.Search
{
    public class DomainObjectSearchResult : IDisposable
    {
        #region Fields



        #endregion

        #region Properties

        public int DomainObjectId { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public DomainObjectSearchResult()
        {

        }



        #endregion

        #region Public Methods

        public virtual void Dispose()
        {

        }

        public override bool Equals(object obj)
        {
            var item = obj as DomainObjectSearchResult;

            if (item == null)
            {
                return false;
            }

            if (DomainObjectId == 0 && item.DomainObjectId == 0)
            {
                return GetHashCode() == item.GetHashCode();
            }
            else
            {
                return DomainObjectId == item.DomainObjectId;
            }
        }

        public virtual NodeSummary GetSummary()
        {
            return new NodeSummary();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
