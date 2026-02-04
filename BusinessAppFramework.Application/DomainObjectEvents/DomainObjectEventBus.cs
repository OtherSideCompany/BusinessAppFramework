using Microsoft.Extensions.DependencyInjection;

namespace BusinessAppFramework.Application.DomainObjectEvents
{
   public class DomainObjectEventBus : IDomainObjectEventBus
   {
      #region Fields

      private readonly IServiceProvider _serviceProvider;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEventBus(IServiceProvider serviceProvider)
      {
         _serviceProvider = serviceProvider;
      }

      #endregion

      #region Public Methods

      public async Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : DomainObjectEvent
      {
         var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();

         foreach (var handler in handlers)
         {
            await handler.HandleAsync(domainEvent);
         }
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
