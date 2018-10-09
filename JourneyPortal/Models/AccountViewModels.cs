using JourneyPortal.Models.Offer;
using JourneyPortal.ViewModels.Offers;
using JourneyPortal.ViewModels.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace JourneyPortal.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class UserRoleViewModel
    {
        [Required(AllowEmptyStrings = false , ErrorMessage = "Pole Rola jest wymagane.")]
        [Display(Name = "Rola")]
        public string UserRole { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamiętaj?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        
        [Display(Name = "Rola użytkownika")]
        public string UserRoles { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać minimum {2} znaki.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false , ErrorMessage = "Imię jest wymagane!")]
        [Display(Name ="Imię")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nazwisko jest wymagane!")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }


        [Display(Name = "Data urodzenia")]
        public DateTime DateOfBirth { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać minimum {2} znaki", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class TravelAgencyInfoViewModel : BaseFormViewModel
    {
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public List<CreateOfferDetailViewModel> OffersList { get; set; }
    }

    public class UserProfileInfo : BaseFormViewModel
    {
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Rola")]
        public string Role { get; set; }

        internal void UpdateFromModel(ApplicationUser u)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            Login = u.UserName;
            Email = u.Email;
            FirstName = u.FirstName;
            LastName = u.LastName;
            DateOfBirth = u.DateOfBirth;

            var userRoles = (from user in context.Users
                             where user.UserName == Login
                             select new
                             {
                                 RoleNames = (from userRole in user.Roles
                                              join role in context.Roles on userRole.RoleId
                                              equals role.Id
                                              select role.Name).ToList()
                             }).ToList();
            Role = userRoles.FirstOrDefault().RoleNames.FirstOrDefault();

            

        }

    }
}
