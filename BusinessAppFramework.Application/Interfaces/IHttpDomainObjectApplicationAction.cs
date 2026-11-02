namespace BusinessAppFramework.Application.Interfaces
{
   public interface IHttpDomainObjectApplicationAction : IDomainObjectApplicationAction
   {
      HttpMethod HttpMethod { get; }
      string? ConfirmationMessageKey { get; }
   }
}
