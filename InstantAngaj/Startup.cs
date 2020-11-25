using InstantAngaj.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using System.Linq;
using Owin;

[assembly: OwinStartupAttribute(typeof(InstantAngaj.Startup))]
namespace InstantAngaj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createAdminUserAndApplicationRoles();
            createInitialDatabaseData();
        }

        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                // Se adauga rolul de User
                var role = new IdentityRole();
                role.Name = "Candidate";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                // Se adauga rolul de User
                var role = new IdentityRole();
                role.Name = "Employer";
                roleManager.Create(role);
            }
        }

        private void createInitialDatabaseData()
        {
            createCities();
        }

        private void createCities()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var cityList = (from city in db.Cities
                            select city
                            ).ToList();

            if (cityList.Count == 0)
            {
                db.Cities.Add(new City
                {
                    CityId = 1,
                    Name = "Bucuresti"
                });

                db.Cities.Add(new City
                {
                    CityId = 2,
                    Name = "Cluj"
                });

                db.Cities.Add(new City
                {
                    CityId = 3,
                    Name = "Timisoara"
                });

                db.Cities.Add(new City
                {
                    CityId = 4,
                    Name = "Iasi"
                });

                db.Cities.Add(new City
                {
                    CityId = 5,
                    Name = "Brasov"
                });

                db.Cities.Add(new City
                {
                    CityId = 6,
                    Name = "Constanta"
                });

                db.Cities.Add(new City
                {
                    CityId = 7,
                    Name = "Braila"
                });

                db.Cities.Add(new City
                {
                    CityId = 8,
                    Name = "Sibiu"
                });

                db.Cities.Add(new City
                {
                    CityId = 9,
                    Name = "Oradea"
                });

                db.Cities.Add(new City
                {
                    CityId = 10,
                    Name = "Craiova"
                });

                db.SaveChanges();
            }
        }
    }
}
