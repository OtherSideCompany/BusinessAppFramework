using Application.DomainObjectEvents;
using Application.Interfaces;
using Application.Mail;
using Application.Relations;
using Domain.Services;
namespace Application.Services
{
   public class DomainObjectServiceDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IUserContext UserContext { get; set; }
      public IReferenceHydrator ReferenceHydrator { get; set; }
      public IDomainObjectFileService DomainObjectFileService { get; }
      public IPasswordService PasswordService { get; }
      public IMailService MailService { get; }
      public IDomainObjectEventBus DomainObjectEventBus { get; }
      public IUserPermissionResolverService UserPermissionResolverService { get; set; }
      public IRelationResolver RelationResolver { get; }
      public IRelationService RelationService { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectServiceDependencies(
          IUserContext userContext,
          IDomainObjectFileService domainObjectFileService,
          IPasswordService passwordService,
          IMailService mailService,
          IDomainObjectEventBus domainObjectEventBus,
          IUserPermissionResolverService userPermissionResolverService,
          IRelationResolver relationResolver,
          IReferenceHydrator referenceHydrator,
          IRelationService relationService)
      {
         UserContext = userContext;
         DomainObjectFileService = domainObjectFileService;
         PasswordService = passwordService;
         MailService = mailService;
         DomainObjectEventBus = domainObjectEventBus;
         UserPermissionResolverService = userPermissionResolverService;
         RelationResolver = relationResolver;
         ReferenceHydrator = referenceHydrator;
         RelationService = relationService;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
