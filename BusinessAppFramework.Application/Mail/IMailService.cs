namespace BusinessAppFramework.Application.Mail
{
   public interface IMailService
   {
      string BuildMailtoUri(MailInfo mail);
   }
}
