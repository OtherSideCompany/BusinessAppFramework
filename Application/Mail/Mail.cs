namespace Application.Mail
{
   public class Mail
   {
      #region Fields



      #endregion

      #region Properties

      public string From { get; set; }
      public string To { get; set; }
      public string Subject { get; set; }
      public string Body { get; set; }
      public List<FileInfo> Attachments { get; set; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public Mail()
      {
         Attachments = new List<FileInfo>();
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
