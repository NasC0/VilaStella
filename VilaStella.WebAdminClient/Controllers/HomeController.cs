using System;
using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using VilaStella.Data;
using VilaStella.Data.Common.Repositories;
using VilaStella.Models;
using AutoMapper.QueryableExtensions;
using VilaStella.WebAdminClient.Areas.Admin.ViewModels;

namespace VilaStella.WebAdminClient.Controllers
{
    public class HomeController : Controller
    {
        private IDeletableRepository<Image> images;
        private IGenericRepositoy<GeneralSettings> settings;

        public HomeController(IDeletableRepository<Image> images, IGenericRepositoy<GeneralSettings> settings)
        {
            this.images = images;
            this.settings = settings;
        }

        public ActionResult Index()
        {
            var currentSetings = this.settings.All()
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            bool areReservationsOpen = currentSetings.AreReservationsOpen;
            return View(areReservationsOpen);
        }

        public ActionResult RenderGallery()
        {
            var images = this.images.All()
                .Where(x => x.IsShown)
                .OrderByDescending(x => x.ID)
                .Project()
                .To<ImageOutputModel>()
                .ToList();

            return PartialView("_Gallery", images);
        }

        public ActionResult RenderReservation()
        {
            return PartialView("_Reservation");
        }
    }
}