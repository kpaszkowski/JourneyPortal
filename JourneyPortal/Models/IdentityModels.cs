using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using JourneyPortal.Models.Forum;
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
            this.OffersApplicationUsers = new HashSet<OffersApplicationUsers>();
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Column(TypeName = "image")]
        public byte[] Avatar { get; set; }

        public virtual ICollection<OffersApplicationUsers> OffersApplicationUsers { get; set; }

        public virtual ICollection<Offers> OwnerOffers { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<OffersComment> OffersComments { get; set; }

        public virtual ICollection<PostsUsers> PostsUsers { get; set; }

    }

}