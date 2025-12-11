using OtherSideCore.Application.DomainObjectEvents;
using OtherSideCore.Application.Factories;
using OtherSideCore.Application.Interfaces;
using OtherSideCore.Application.Mail;
using OtherSideCore.Application.Relations;
using OtherSideCore.Domain.Services;
namespace OtherSideCore.Application.Services
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
            IReferenceHydrator referenceHydrator)
        {
            UserContext = userContext;
            DomainObjectFileService = domainObjectFileService;
            PasswordService = passwordService;
            MailService = mailService;
            DomainObjectEventBus = domainObjectEventBus;
            UserPermissionResolverService = userPermissionResolverService;
            RelationResolver = relationResolver;
            ReferenceHydrator = referenceHydrator;
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}
