using Domain.DomainObjects;

namespace Application.Interfaces
{
   public interface IEntity
   {
      int Id { get; set; }
      HistoryInfo HistoryInfo { get; set; }
   }
}
