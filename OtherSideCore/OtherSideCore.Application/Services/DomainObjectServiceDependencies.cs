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

      public IUserContext UserContext { get; set; }
      public IDomainObjectServiceFactory DomainObjectServiceFactory { get; set; }
      public IUserDialogService UserDialogService { get; set; }
      public IDomainObjectFileService DomainObjectFileService { get; set; }
      public IPasswordService PasswordService { get; set; }
      public IMailService MailService { get; set; }
      public IDomainObjectSearchFactory DomainObjectSearchFactory { get; set; }
      public IDomainObjectEventBus DomainObjectEventBus { get; set; }

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
