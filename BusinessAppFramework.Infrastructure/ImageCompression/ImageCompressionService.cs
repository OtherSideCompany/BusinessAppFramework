using BusinessAppFramework.Application.Services;
using ImageMagick;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace BusinessAppFramework.Infrastructure.ImageCompression
{
   public class ImageCompressionService : IImageCompressionService
   {
      #region Fields

      private const int _defaultKbThreshold = 500;

      #endregion

      #region Properties



      #endregion

      #region Commands



      #endregion

      #region Constructor

      public ImageCompressionService()
      {

      }

      #endregion

      #region Public Methods

      public void CompressAndSaveImageAsJpeg(string sourceFilePath, string destinationFilePath)
      {
         var sourceFileInfo = new FileInfo(sourceFilePath);

         if (IsJpegImage(sourceFilePath) && sourceFileInfo.Length / 1000 <= _defaultKbThreshold)
         {

            File.Copy(sourceFilePath, destinationFilePath);
         }
         else
         {
            MemoryStream memoryStream;

            if (!IsHeicImage(sourceFilePath))
            {
               memoryStream = new MemoryStream(File.ReadAllBytes(sourceFilePath));
            }
            else
            {
               using (IMagickImage image = new MagickImage(sourceFilePath))
               {
                  memoryStream = new MemoryStream();
                  image.Write(memoryStream, MagickFormat.Jpeg);
               }
            }

            using (var bitmap = new Bitmap(memoryStream))
            {
               ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

               Encoder myEncoder = Encoder.Quality;

               EncoderParameters myEncoderParameters = new EncoderParameters(1);

               for (int quality = 100; quality >= 0; quality -= 10)
               {
                  EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                  myEncoderParameters.Param[0] = myEncoderParameter;

                  using (var destMemoryStream = new MemoryStream())
                  {
                     bitmap.Save(destMemoryStream, jpgEncoder, myEncoderParameters);

                     if (destMemoryStream.Length / 1000 <= _defaultKbThreshold)
                     {
                        using (var fileStream = new FileStream(destinationFilePath, FileMode.CreateNew))
                        {
                           destMemoryStream.WriteTo(fileStream);
                        }
                        break;
                     }

                     destMemoryStream.Close();
                     destMemoryStream.Dispose();
                  }
               }
            }

            memoryStream.Close();
            memoryStream.Dispose();
         }
      }

      public async Task<byte[]> CompressPngImageAsync(Stream inputPng, int maxWidth, int maxHeight)
      {
         using var image = await SixLabors.ImageSharp.Image.LoadAsync(inputPng);

         if (image.Width > maxWidth || image.Height > maxHeight)
         {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
               Mode = ResizeMode.Max,
               Size = new SixLabors.ImageSharp.Size(maxWidth, maxHeight)
            }));
         }

         using var output = new MemoryStream();

         var encoder = new PngEncoder
         {
            CompressionLevel = PngCompressionLevel.BestCompression
         };

         await image.SaveAsync(output, encoder);
         return output.ToArray();
      }

      #endregion

      #region Private Methods

      private bool IsJpegImage(string fileName)
      {
         return Path.GetExtension(fileName).ToLower().Equals(".jpg") ||
                Path.GetExtension(fileName).ToLower().Equals(".jpeg");
      }

      private bool IsHeicImage(string fileName)
      {
         return Path.GetExtension(fileName).ToLower().Equals(".heic") ||
                Path.GetExtension(fileName).ToLower().Equals(".heif");
      }

      private ImageCodecInfo GetEncoder(ImageFormat format)
      {
         ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

         foreach (ImageCodecInfo codec in codecs)
         {
            if (codec.FormatID == format.Guid)
            {
               return codec;
            }
         }

         return null;
      }      

      #endregion
   }
}
