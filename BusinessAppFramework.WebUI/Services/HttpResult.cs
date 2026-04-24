namespace BusinessAppFramework.WebUI.Services
{
    public record HttpResult<T>
    (
        bool Success,
        T? Data,
        string? ErrorMessage,
        int? StatusCode = null
    );

    public record HttpResult(bool Success, string? ErrorMessage, int? StatusCode);
}
