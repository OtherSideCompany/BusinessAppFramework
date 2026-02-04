namespace Application.Services
{
   public interface ICurrentUserService
   {
      int? UserId { get; }
      string? AuthenticationProviderId { get; }
   }
}
