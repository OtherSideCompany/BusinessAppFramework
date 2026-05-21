using BusinessAppFramework.Application.Interfaces;
using ImageMagick;
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
            using var image = new MagickImage();
            await Task.Run(() => image.Read(inputPng));

            if (image.Width > (uint)maxWidth || image.Height > (uint)maxHeight)
            {
                image.Resize(new MagickGeometry((uint)maxWidth, (uint)maxHeight) { Greater = true });
            }

            image.Format = MagickFormat.Png;
            image.Quality = 100;
            image.SetCompression(CompressionMethod.Zip);

            using var output = new MemoryStream();
            await Task.Run(() => image.Write(output));
            return output.ToArray();
        }

        public byte[] CompressToJpeg(byte[] input, int maxSize = 256, int quality = 80)
        {
            using var image = new MagickImage(input);

            image.Resize(new MagickGeometry((uint)maxSize, (uint)maxSize) { Greater = true });

            image.Format = MagickFormat.Jpeg;
            image.Quality = (uint)quality;

            using var output = new MemoryStream();
            image.Write(output);
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
