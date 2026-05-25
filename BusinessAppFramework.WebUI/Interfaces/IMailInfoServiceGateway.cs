using BusinessAppFramework.Application.Mail;
using BusinessAppFramework.Domain.DomainObjects;

namespace BusinessAppFramework.WebUI.Interfaces
{
    public interface IMailInfoServiceGateway<T> where T : DomainObject, new()
    {
        Task<MailInfo?> GetMailInfoAsync(int domainObjectId, string cultureInfo, CancellationToken ct = default);
    }
}
