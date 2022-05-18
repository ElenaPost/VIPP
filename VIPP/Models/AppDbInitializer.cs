using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace VIPP.Models
{
	public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
	{
		protected override void Seed(ApplicationDbContext context)
		{
			var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

			var adminRole = new IdentityRole { Name = "admin" };

			roleManager.Create(adminRole);

			var admin = new ApplicationUser { UserName = "Elena", Email = "postolskaya.elena@gmail.com", DateOfBirth = new DateTime(1985, 12, 21), Gender = "женский" };
			string password = "admin_Elena1";
			var result = userManager.Create(admin, password);

			if(result.Succeeded)
			{
				userManager.AddToRole(admin.Id, adminRole.Name);
			}

			admin = new ApplicationUser { UserName = "Irina", Email = "postolskaya.elena@gmail.com", DateOfBirth = new DateTime(1985, 12, 21), Gender = "женский" };
			password = "admin_Irina1";
			result = userManager.Create(admin, password);

			if (result.Succeeded)
			{
				userManager.AddToRole(admin.Id, adminRole.Name);
			}

			base.Seed(context);
		}
	}
}