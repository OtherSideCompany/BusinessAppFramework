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

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public DomainObjectServiceDependencies(
         IUserContext _userContext,
         IUserDialogService _userDialogService,
         IDomainObjectFileService _domainObjectFileService,
         IPasswordService _passwordService,
         IMailService _mailService,
         IDomainObjectSearchFactory _domainObjectSearchFactory)
      {
         UserContext = _userContext;
         UserDialogService = _userDialogService;
         DomainObjectFileService = _domainObjectFileService;
         PasswordService = _passwordService;
         MailService = _mailService;
         DomainObjectSearchFactory = _domainObjectSearchFactory;
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
