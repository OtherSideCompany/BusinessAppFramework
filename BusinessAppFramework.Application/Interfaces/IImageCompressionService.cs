namespace BusinessAppFramework.Application.Interfaces
{
   public interface IImageCompressionService
   {
      void CompressAndSaveImageAsJpeg(string sourceFilePath, string destinationFilePath);
      Task<byte[]> CompressPngImageAsync(Stream inputPng, int maxWidth, int maxHeight);
   }
}
