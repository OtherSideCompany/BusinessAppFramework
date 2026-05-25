using BusinessAppFramework.Application.Mail;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Application.Interfaces
{
    public interface IMailInfoService
    {
        Task<MailInfo?> GetMailInfoAsync(int domainObjectId, string cultureInfo, CancellationToken ct = default);
    }
}
