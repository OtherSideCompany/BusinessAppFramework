namespace BusinessAppFramework.Application.Interfaces
{
   public interface IDomainObjectNavigationApplicationAction : IDomainObjectApplicationAction
   {
        string BuildRoute(int domainObjectId);
    }
}
