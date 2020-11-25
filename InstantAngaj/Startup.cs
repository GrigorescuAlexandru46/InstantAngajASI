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
            createDomains();
            createDegrees();
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

        private void createDomains()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var domainList = (from domain in db.Domains
                              select domain
                             ).ToList();

            if (domainList.Count == 0)
            {
                db.Domains.Add(new Domain
                {
                    DomainId = 1,
                    Name = "Constructii"
                });

                db.Domains.Add(new Domain
                {
                    DomainId = 2,
                    Name = "Agricultura"
                });

                db.Domains.Add(new Domain
                {
                    DomainId = 3,
                    Name = "Logistica"
                });

                db.Domains.Add(new Domain
                {
                    DomainId = 4,
                    Name = "IT"
                });

                db.Domains.Add(new Domain
                {
                    DomainId = 5,
                    Name = "Contabilitate"
                });

                db.Domains.Add(new Domain
                {
                    DomainId = 6,
                    Name = "Consultanta"
                });

                db.Domains.Add(new Domain
                {
                    DomainId = 7,
                    Name = "Juridic"
                });

                db.SaveChanges();
            }
        }

        private void createDegrees()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var degreeList = (from degree in db.Degrees
                              select degree
                             ).ToList();

            if (degreeList.Count == 0)
            {
                db.Degrees.Add(new Degree
                {
                    DegreeId = 1,
                    Name = "Diploma de absolvire a invatamantului gimnazial"
                });

                db.Degrees.Add(new Degree
                {
                    DegreeId = 2,
                    Name = "Diploma de absolvire a liceului "
                });

                db.Degrees.Add(new Degree
                {
                    DegreeId = 3,
                    Name = "Diploma de bacalaureat"
                });

                db.Degrees.Add(new Degree
                {
                    DegreeId = 4,
                    Name = "Diploma de licenta"
                });

                db.Degrees.Add(new Degree
                {
                    DegreeId = 5,
                    Name = "Diploma de master"
                });

                db.Degrees.Add(new Degree
                {
                    DegreeId = 6,
                    Name = "Diploma de doctor"
                });

                db.SaveChanges();
            }
        }
    }
}
