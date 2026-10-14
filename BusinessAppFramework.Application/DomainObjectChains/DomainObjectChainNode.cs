namespace BusinessAppFramework.Application.DomainObjectChains
{
    public class DomainObjectChainNode
    {
        public string DomainObjectTypeKey { get; set; } = string.Empty;
        public int DomainObjectId { get; set; }
        public string LabelKey { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public bool CanAccess { get; set; }
    }
}
