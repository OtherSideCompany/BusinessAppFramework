namespace BusinessAppFramework.Application.Services
{
   public interface ICurrentUserService
   {
      int? UserId { get; }
      string? AuthenticationPurchaseId { get; }
   }
}
