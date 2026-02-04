namespace Application.DomainObjectEvents
{
   public interface IDomainObjectEventBus
   {
      Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : DomainObjectEvent;
   }
}
