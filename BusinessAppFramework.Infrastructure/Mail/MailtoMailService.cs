using BusinessAppFramework.Application.Mail;
using System;
using System.Linq;

namespace BusinessAppFramework.Infrastructure.Mail
{
   public class MailtoMailService : IMailService
   {
      #region Fields



      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public MailtoMailService()
      {

      }

      #endregion

      #region Public Methods

      public string BuildMailtoUri(Application.Mail.MailInfo mail)
      {
         string to = string.Join(",", (mail.To ?? string.Empty)
            .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(address => Uri.EscapeDataString(address).Replace("%40", "@")));
         string subject = Uri.EscapeDataString(mail.Object ?? string.Empty);
         string body = Uri.EscapeDataString((mail.Body ?? string.Empty).Replace("\r\n", "\n").Replace("\n", "\r\n"));

         return $"mailto:{to}?subject={subject}&body={body}";
      }

      #endregion

      #region Private Methods



      #endregion
   }
}
