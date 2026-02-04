namespace Application.Services
{
   public interface IImageCompressionService
   {
      void CompressAndSaveImageAsJpeg(string sourceFilePath, string destinationFilePath);
   }
}
