using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using JourneyPortal.Models.Offer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JourneyPortal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser() :base()
        {
            this.AssignedOffers = new HashSet<Offers>();
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<Offers> AssignedOffers { get; set; }

        public virtual ICollection<Offers> OwnerOffers { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("JourneyPortalDBWork", throwIfV1Schema: false)
        {
        }

        public DbSet<Offers> Offers { get; set; }

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
                        .HasMany<ApplicationUser>(s => s.AssignedUsers)
                        .WithMany(x => x.AssignedOffers);

        }
    }
}