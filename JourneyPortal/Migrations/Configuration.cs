namespace JourneyPortal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JourneyPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "JourneyPortal.Models.ApplicationDbContext";
        }

        protected override void Seed(JourneyPortal.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                
            }
        }
    }
}
