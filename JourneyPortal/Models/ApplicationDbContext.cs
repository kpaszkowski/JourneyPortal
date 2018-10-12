using JourneyPortal.Models.Forum;
using JourneyPortal.Models.Offer;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JourneyPortal.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("JourneyPortalDWork", throwIfV1Schema: false)
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<Offers> Offers { get; set; }
        public DbSet<OffersApplicationUsers> OffersApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<OffersComment> OffersComments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Offers>()
                        .HasRequired(x => x.TravelAgencyOwner)
                        .WithMany(c => c.OwnerOffers)
                        .HasForeignKey<string>(x => x.TravelAgencyOwnerId)
                        .WillCascadeOnDelete(false);
            modelBuilder.Entity<Offers>()
                .HasMany<OffersApplicationUsers>(x => x.OffersApplicationUsers)
                .WithRequired(x => x.Offers)
                .HasForeignKey<int>(x => x.OfferId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<OffersApplicationUsers>(x => x.OffersApplicationUsers)
                .WithRequired(x => x.ApplicationUser)
                .HasForeignKey<string>(x => x.ApplicationUserId);
        }
    }
}