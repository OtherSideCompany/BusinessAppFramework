namespace BusinessAppFramework.Application.Services
{
   public class ManagedFile
   {
      #region Fields



      #endregion

      #region Properties

      public FileInfo PhysicalFile { get; }
      public MemoryStream InMemoryContent { get; }
      public bool IsInMemory => InMemoryContent != null;
      public string FileName { get; }
      public string FileExtension { get; }

      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ManagedFile(FileInfo fileInfo)
      {
         PhysicalFile = fileInfo;
         FileName = fileInfo.Name;
         FileExtension = fileInfo.Extension;
      }

      public ManagedFile(string fileName, MemoryStream content)
      {
         FileName = fileName;
         InMemoryContent = content;
         FileExtension = Path.GetExtension(fileName);
      }

      #endregion

      #region Public Methods



      #endregion

      #region Private Methods



      #endregion
   }
}
