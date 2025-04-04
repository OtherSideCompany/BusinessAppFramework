using OtherSideCore.Application.Factories;

namespace OtherSideCore.Application.DomainObjectEvents
{
   public abstract class DomainObjectEventPublisher : IDomainObjectEventPublisher
   {
      #region Fields

      protected IDomainObjectServiceFactory _domainObjectServiceFactory;

      private readonly List<IEventHandlerBase> _handlers = new();

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectEventPublisher(IDomainObjectServiceFactory domainObjectServiceFactory)
      {
         _domainObjectServiceFactory = domainObjectServiceFactory;
      }

      #endregion

      #region Public Methods

      

      public async Task PublishAsync<TEvent>(TEvent domainEvent)
      {
         foreach (var handler in _handlers.OfType<IEventHandler<TEvent>>())
         {
            await handler.HandleAsync(domainEvent);
         }
      }

      #endregion

      #region Private Methods

      protected void RegisterHandler(IEventHandlerBase handler)
      {
         _handlers.Add(handler);
      }

      #endregion
   }
}
