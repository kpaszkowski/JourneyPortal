using JourneyPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JourneyPortal.Startup))]
namespace JourneyPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserRoles();
        }

        public void CreateUserRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "superadmin";
                user.Email = "kpaszkowski1000@gmail.com";
                string userPWD = "123456";
                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
                var user = new ApplicationUser();
                user.UserName = "user";
                user.Email = "kpaszkowski1000@gmail.com";
                string userPWD = "123456";
                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "User");
                }
            }
            if (!roleManager.RoleExists("TravelAgency"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "TravelAgency";
                roleManager.Create(role);
                var user = new ApplicationUser();
                user.UserName = "travelagency";
                user.Email = "kpaszkowski1000@gmail.com";
                string userPWD = "123456";
                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "TravelAgency");
                }
            }
            if (!roleManager.RoleExists("Proprietor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Proprietor";
                roleManager.Create(role);
                var user = new ApplicationUser();
                user.UserName = "proprietor";
                user.Email = "kpaszkowski1000@gmail.com";
                string userPWD = "123456";
                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Proprietor");
                }
            }
        }
    }
}
