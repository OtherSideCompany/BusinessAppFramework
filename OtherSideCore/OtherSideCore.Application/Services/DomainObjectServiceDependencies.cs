using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Mail;
using OtherSideCore.Appplication.Services;
using OtherSideCore.Domain.Services;
namespace OtherSideCore.Application.Services
{
   public class DomainObjectServiceDependencies
   {
      #region Fields



      #endregion

      #region Properties

      public IUserContext UserContext { get; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; set; }
      public IUserDialogService UserDialogService { get; }
      public IDomainObjectFileService DomainObjectFileService { get; }
      public IPasswordService PasswordService { get; }
      public IMailService MailService { get; }
      public IDomainObjectSearchFactory DomainObjectSearchFactory { get; }
      public IDomainObjectEventBus DomainObjectEventBus { get; }
      public IUserPermissionService UserPermissionService { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectServiceDependencies(
         IUserContext userContext,
         IUserDialogService userDialogService,
         IDomainObjectFileService domainObjectFileService,
         IPasswordService passwordService,
         IMailService mailService,
         IDomainObjectSearchFactory domainObjectSearchFactory,
         IDomainObjectEventBus domainObjectEventBus)
      {
         UserContext = userContext;
         UserDialogService = userDialogService;
         DomainObjectFileService = domainObjectFileService;
         PasswordService = passwordService;
         MailService = mailService;
         DomainObjectSearchFactory = domainObjectSearchFactory;
         DomainObjectEventBus = domainObjectEventBus;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
