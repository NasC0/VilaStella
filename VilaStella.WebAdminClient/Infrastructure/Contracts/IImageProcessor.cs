using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VilaStella.WebAdminClient.Infrastructure.Contracts
{
    public interface IImageProcessor
    {
        byte[] GetImageByteArray(Stream inputStream);

        byte[] ResizeImageByteArray(byte[] image, int percentResize);

        byte[] ResizeImageByteArray(byte[] image, int width, int height);
    }
}