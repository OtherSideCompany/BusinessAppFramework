namespace Application.Interfaces
{
   public interface IHttpDomainObjectApplicationAction : IDomainObjectApplicationAction
   {
      HttpMethod HttpMethod { get; }
   }
}
