using JourneyPortal.Models;
using JourneyPortal.Models.Trips;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

[assembly: OwinStartupAttribute(typeof(JourneyPortal.Startup))]
namespace JourneyPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            ConfigureAuth(app);
            CreateUserRoles(userManager,roleManager);
            if (!bool.Parse(ConfigurationManager.AppSettings["SeedDatabase"]))
            {
                CreateUsers(userManager, roleManager);
                CreateAtractionsAndHotels();
                CreateOffers();
                CreateForum();
            }
        }

        private void CreateUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var user = new ApplicationUser();
            user.UserName = "user1";
            user.Email = "kpaszkowski1000@gmail.com";
            user.FirstName = "Jan";
            user.LastName = "Jan";
            user.DateOfBirth = new DateTime(1997, 2, 11);
            string userPWD = "123456";
            var chkUser = userManager.Create(user, userPWD);
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "User");
            }

            user = new ApplicationUser();
            user.UserName = "user2";
            user.Email = "kpaszkowski1000@gmail.com";
            user.FirstName = "Franek";
            user.LastName = "Dolas";
            user.DateOfBirth = new DateTime(1986, 4, 17);
            userPWD = "123456";
            chkUser = userManager.Create(user, userPWD);
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "User");
            }

            user = new ApplicationUser();
            user.UserName = "travelagency2";
            user.Email = "kpaszkowski1000@gmail.com";
            user.FirstName = "Marian";
            user.LastName = "Paździoch";
            user.DateOfBirth = new DateTime(1969, 6, 27);
            userPWD = "123456";
            chkUser = userManager.Create(user, userPWD);
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "TravelAgency");
            }
        }

        private void CreateForum()
        {

        }

        private void CreateOffers()
        {

        }

        private void CreateAtractionsAndHotels()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                    Encoding enc = Encoding.Default;
                    string directory = HttpContext.Current.Server.MapPath("~/App_Data/") + "baza.txt";
                    StreamReader file = new StreamReader(directory, enc);
                    string[] data = file.ReadLine().Split(null);
                    var NumberOfAtractions = Int32.Parse(data[0]);

                    for (int i = 0; i < NumberOfAtractions; i++)
                    {
                        string[] data2 = file.ReadLine().Split(',');

                        if (data2[4] == "Hotel")
                        {
                            Hotel hotel = new Hotel
                            {
                                X = double.Parse(data2[0], CultureInfo.InvariantCulture),
                                Y = double.Parse(data2[1], CultureInfo.InvariantCulture),
                                Name = data2[2],
                                Description = "Super hotel",
                                CostPerNight = 999,
                                IsActive = true,
                                OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                            };
                            context.Hotels.Add(hotel);
                        }
                        else
                        {
                            Atraction atraction = new Atraction
                            {
                                X = double.Parse(data2[0], CultureInfo.InvariantCulture),
                                Y = double.Parse(data2[1], CultureInfo.InvariantCulture),
                                Name = data2[2],
                                Description = "Super atrakcja",
                                Cost = 999,
                                IsActive = true,
                                TimeOfSightseeing = 2,
                                OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                                Type = data2[4],
                            };
                            context.Atractions.Add(atraction);
                        }
                    }

                    #region ...
                    /*
                    var hotel = new Hotel
                    {
                        Name = "San marino hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "ce5b8f44-5a3e-4318-bacd-0de42aa6039d",
                        X = 43.946295,
                        Y = 12.451567
                    };
                    context.Hotels.Add(hotel);

                    hotel = new Hotel
                    {
                        Name = "Macedonia hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "ce5b8f44-5a3e-4318-bacd-0de42aa6039d",
                        X = 42.008514,
                        Y = 21.392983
                    };
                    context.Hotels.Add(hotel);

                    hotel = new Hotel
                    {
                        Name = "Dania hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "ce5b8f44-5a3e-4318-bacd-0de42aa6039d",
                        X = 55.676210,                        
                        Y = 12.586169
                    };
                    context.Hotels.Add(hotel);

                    hotel = new Hotel
                    {
                        Name = "Francja hotel",
                        Description = "5 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "ce5b8f44-5a3e-4318-bacd-0de42aa6039d",
                        X = 48.852768,
                        Y = 2.335718,
                    };
                    context.Hotels.Add(hotel);
                    hotel = new Hotel
                    {
                        Name = "Hiszpania Barcelona hotel",
                        Description = "4 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "ce5b8f44-5a3e-4318-bacd-0de42aa6039d",
                        X = 41.369413,
                        Y = 2.169887
                    };
                    context.Hotels.Add(hotel);
                    hotel = new Hotel
                    {
                        Name = "Londyn hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                        X = 51.724359,
                        Y = -0.205296
                    };
                    context.Hotels.Add(hotel);
                    hotel = new Hotel
                    {
                        Name = "Norwegia hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                        X = 61.693119,
                        Y = 7.861526
                    };
                    context.Hotels.Add(hotel);
                    hotel = new Hotel
                    {
                        Name = "Wyspy owcze hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                        X = 62.015607,
                        Y = -6.937803
                    };
                    context.Hotels.Add(hotel);
                    hotel = new Hotel
                    {
                        Name = "Tunezja hotel",
                        Description = "3 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                        X = 36.859779,
                        Y = 10.156103
                    };
                    context.Hotels.Add(hotel);
                    hotel = new Hotel
                    {
                        Name = "Turcja hotel",
                        Description = "2 gwiazdki",
                        CostPerNight = 999,
                        IsActive = true,
                        OwnerId = "8e136864-9ccc-481b-8390-039c11be8a25",
                        X = 39.945337,
                        Y = 32.800351
                    };
                    context.Hotels.Add(hotel);
                    */
                    #endregion

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void CreateUserRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "superadmin";
                user.Email = "kpaszkowski1000@gmail.com";
                user.FirstName = "krzycho";
                user.LastName = "krzycho";
                user.DateOfBirth = new DateTime(1995, 6, 1);
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
                user.FirstName = "jan";
                user.LastName = "jan";
                user.DateOfBirth = new DateTime(1993, 4, 15);
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
                user.FirstName = "pjoter";
                user.LastName = "pjoter";
                user.DateOfBirth = new DateTime(1997, 11, 9);
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
                user.FirstName = "franek";
                user.LastName = "franek";
                user.DateOfBirth = new DateTime(1992, 5, 26);
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
