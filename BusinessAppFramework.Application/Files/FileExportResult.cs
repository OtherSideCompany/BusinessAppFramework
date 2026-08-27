namespace BusinessAppFramework.Application.Files
{
    public record FileExportResult(byte[] Content, string ContentType, string FileName);
}
