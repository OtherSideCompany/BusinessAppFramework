using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Documents
{
    public class DocumentCategory<TDocument> where TDocument : IDocument
    {
        public string CategoryLabelKey { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string ParentReferenceNumber { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string? NavigationKey { get; set; }
        public string DocumentsRelationKey { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public List<TDocument> Documents { get; set; } = new();
    }
}
