using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using VilaStella.Models;

namespace VilaStella.Data
{
    public interface IDbContext
    {
        IDbSet<Image> Images { get; set; }

        IDbSet<Reservation> Reservations { get; set; }

        IDbSet<GeneralSettings> GeneralSettings { get; set; }

        IDbSet<Email> EmailTemplates { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
    }
}
