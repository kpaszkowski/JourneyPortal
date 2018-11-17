using JourneyPortal.Models.Forum;
using JourneyPortal.Models.Offer;
using JourneyPortal.Models.Trips;
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
        public ApplicationDbContext(): base()
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
        public DbSet<PostsUsers> PostsUsers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Atraction> Atractions { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<GlobalMessage> GlobalMessages { get; set; }

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

            modelBuilder.Entity<Post>()
                .HasMany<PostsUsers>(x => x.PostsUsers)
                .WithRequired(x => x.Post)
                .HasForeignKey<int>(x => x.PostId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<PostsUsers>(x => x.PostsUsers)
                .WithRequired(x => x.ApplicationUser)
                .HasForeignKey<string>(x => x.ApplicationUserId);

            modelBuilder.Entity<Atraction>()
                .HasMany(x => x.Trips)
                .WithMany(x => x.Atractions);

            modelBuilder.Entity<Offers>()
            .HasMany<OffersComment>(s => s.OffersComments)
            .WithOptional()
            .WillCascadeOnDelete(true);

        }
    }
}