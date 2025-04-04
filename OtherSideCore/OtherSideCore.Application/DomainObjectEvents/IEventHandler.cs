namespace OtherSideCore.Application.DomainObjectEvents
{
   public interface IEventHandler<TEvent> : IEventHandlerBase
   {
      Task HandleAsync(TEvent domainEvent);
   }
}
