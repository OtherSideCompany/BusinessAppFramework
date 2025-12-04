using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Mail;
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
        public IDomainObjectFileService DomainObjectFileService { get; }
        public IPasswordService PasswordService { get; }
        public IMailService MailService { get; }
        public IDomainObjectEventBus DomainObjectEventBus { get; }
        public IUserPermissionResolverService UserPermissionResolverService { get; set; }

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
            IUserPermissionResolverService userPermissionResolverService)
        {
            UserContext = userContext;
            DomainObjectFileService = domainObjectFileService;
            PasswordService = passwordService;
            MailService = mailService;
            DomainObjectEventBus = domainObjectEventBus;
            UserPermissionResolverService = userPermissionResolverService;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
