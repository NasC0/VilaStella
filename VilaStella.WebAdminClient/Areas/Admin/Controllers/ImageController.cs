using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;

namespace VilaStella.WebAdminClient.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class ImageController : Controller
    {
        public IDeletableRepository<Image> images;

        public ImageController(IDeletableRepository<Image> images)
        {
            this.images = images;
        }

        // GET: Admin/Image
        public ActionResult Index()
        {
            var images = this.images.All()
                .OrderByDescending(x => x.ID)
                .Project()
                .To<ImageOutputModel>()
                .ToList();

            return View(images);
        }

        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                int id = 0;

                foreach (string file in Request.Files)
                {
                    var currentFile = Request.Files[file];

                    var byteStream = GetImageByteArray(currentFile.InputStream);
                    string fileName = currentFile.FileName;
                    string contentType = currentFile.ContentType;

                    var currentImage = new Image
                    {
                        FileName = fileName,
                        Picture = byteStream,
                        ImageType = contentType,
                        IsShown = true
                    };

                    this.images.Add(currentImage);
                    this.images.SaveChanges();

                    id = currentImage.ID;
                }

                return RedirectToAction("RenderImage", new { id = id });
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult UpdateContent(int id, string caption)
        {
            if (caption != null)
            {
                var image = this.images.GetById(id);

                if (image.Caption == caption)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified);
                }

                image.Caption = caption;
                images.Update(image);
                images.SaveChanges();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public ActionResult ShowImage(int id, bool showImage)
        {
            var image = this.images.GetById(id);

            if (image != null)
            {
                image.IsShown = showImage;

                this.images.Update(image);
                this.images.SaveChanges();
                
                var imageShown = new {
                    IsShown = image.IsShown
                };

                return Json(imageShown);
            }

            return Json(false);
        }

        [HttpPost]
        public ActionResult DeleteImage(int id)
        {
            var image = this.images.GetById(id);

            if (image != null)
            {
                this.images.Delete(image);
                this.images.SaveChanges();

                return Json(true);
            }

            return Json(false);
        }

        [AllowAnonymous]
        public FileContentResult GetImage(int id)
        {
            var image = this.images
                .GetById(id);

            if (image != null)
            {
                return new FileContentResult(image.Picture, image.ImageType);
            }
            else
            {
                return null;
            }
        }

        [AllowAnonymous]
        public ActionResult RenderImage(int id)
        {
                var image = this.images.All()
                    .Where(x => x.ID == id)
                    .Project()
                    .To<ImageOutputModel>()
                    .FirstOrDefault();

            return PartialView("_GalleryImage", image);
        }

        private byte[] GetImageByteArray(System.IO.Stream inputStream)
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
    }
}