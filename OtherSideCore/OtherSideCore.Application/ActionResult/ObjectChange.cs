using OtherSideCore.Application.Search;

namespace OtherSideCore.Contracts.ActionResult
{
    public class ObjectChange
    {
        public int DomainObjectId { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}
