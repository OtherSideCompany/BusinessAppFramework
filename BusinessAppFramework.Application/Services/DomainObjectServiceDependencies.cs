using BusinessAppFramework.Application.DomainObjectEvents;
using BusinessAppFramework.Application.Interfaces;
using BusinessAppFramework.Application.Mail;
using BusinessAppFramework.Application.Relations;
using BusinessAppFramework.Domain.Services;

namespace BusinessAppFramework.Application.Services
{
    public class DomainObjectServiceDependencies
    {
        #region Fields



        #endregion

        #region Properties

        public ICurrentUserService CurrentUserService { get; set; }
        public IReferenceHydrator ReferenceHydrator { get; set; }
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
            ICurrentUserService currentUserService,
            IPasswordService passwordService,
            IMailService mailService,
            IDomainObjectEventBus domainObjectEventBus,
            IUserPermissionResolverService userPermissionResolverService,
            IRelationResolver relationResolver,
            IReferenceHydrator referenceHydrator,
            IRelationService relationService)
        {
            CurrentUserService = currentUserService;
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
