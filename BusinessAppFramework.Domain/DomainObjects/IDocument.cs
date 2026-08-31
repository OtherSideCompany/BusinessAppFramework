namespace BusinessAppFramework.Domain.DomainObjects
{
    public interface IDocument
    {
        int Id { get; }
        string FileName { get; }
        string ContentType { get; }
        long ByteSize { get; }
        string CreatedByName { get; }
    }
}
