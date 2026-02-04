using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.Application.Interfaces
{
   public interface IEntity
   {
      int Id { get; set; }
      HistoryInfo HistoryInfo { get; set; }
   }
}
