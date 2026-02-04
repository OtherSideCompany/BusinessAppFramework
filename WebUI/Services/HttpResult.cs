namespace WebUI.Services
{
   public record HttpResult<T>
   (
       bool Success,
       T? Data,
       string? ErrorMessage,
       int? StatusCode = null
   );
}
