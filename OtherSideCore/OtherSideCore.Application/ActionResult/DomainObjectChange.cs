using OtherSideCore.Application.Search;

namespace OtherSideCore.Contracts.ActionResult
{
    public class DomainObjectChange
    {
        public int DomainObjectId { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}
