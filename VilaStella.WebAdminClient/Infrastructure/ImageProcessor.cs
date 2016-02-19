using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using VilaStella.WebAdminClient.Infrastructure.Contracts;

namespace VilaStella.WebAdminClient.Infrastructure
{
    public class ImageProcessor : IImageProcessor
    {
        public byte[] GetImageByteArray(Stream inputStream)
        {
            byte[] resultArray;

            using (var stream = inputStream)
            {
                var memoryStream = stream as MemoryStream;

                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                }

                resultArray = memoryStream.ToArray();
            }

            return resultArray;
        }

        public byte[] ResizeImageByteArray(byte[] image, int percentResize)
        {
            Image convertedImage = GetImageFromByteArray(image);
            int width = (int)(convertedImage.Width * (percentResize * 0.01));
            int height = (int)(convertedImage.Height * (percentResize * 0.01));
            return ResizeImage(convertedImage, width, height);
        }

        public byte[] ResizeImageByteArray(byte[] image, int width, int height)
        {
            Image convertedImage = GetImageFromByteArray(image);
            return ResizeImage(convertedImage, width, height);
        }

        private byte[] ResizeImage(Image image, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Image thumbnail = new Bitmap(width, height, image.PixelFormat);

                Graphics thumbnailGraphics = Graphics.FromImage(thumbnail);
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

                Rectangle imageRectangle = new Rectangle(0, 0, width, height);
                thumbnailGraphics.DrawImage(image, imageRectangle);

                thumbnail.Save(stream, ImageFormat.Jpeg);

                return stream.ToArray();
            }
        }

        private Image GetImageFromByteArray(byte[] image)
        {
            using (MemoryStream imageStream = new MemoryStream(image))
            {
                Image convertedImage = Image.FromStream(imageStream);
                return convertedImage;
            }
        }
    }
}