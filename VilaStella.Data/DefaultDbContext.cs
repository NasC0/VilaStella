using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VilaStella.Data.Migrations;
using VilaStella.Models;
using System.Linq;
using VilaStella.Data.Common.Models;

namespace VilaStella.Data
{
    public class DefaultDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        public DefaultDbContext()
            : base("VilaStella", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DefaultDbContext, Configuration>());
        }

        public static DefaultDbContext Create()
        {
            return new DefaultDbContext();
        }

        public virtual IDbSet<Image> Images { get; set; }

        public virtual IDbSet<Reservation> Reservations { get; set; }

        public virtual IDbSet<GeneralSettings> GeneralSettings { get; set; }

        public virtual IDbSet<Email> EmailTemplates { get; set; }

        public virtual IDbSet<ForgottenPassword> ForgottenPasswords { get; set; } 

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                        .Property(x => x.From)
                        .HasColumnType("date");

            modelBuilder.Entity<Reservation>()
                        .Property(x => x.To)
                        .HasColumnType("date");

            base.OnModelCreating(modelBuilder);
        }
    }
}
