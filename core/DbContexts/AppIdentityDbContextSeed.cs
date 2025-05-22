﻿using Core.IdentityEntities;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DbContexts
{
	public class AppIdentityDbContextSeed
	{
		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			foreach (var roleName in AppRoles.AllRoles)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
		}

		public static async Task SeedDefaultAdminUserAsync(UserManager<AppUser> userManager)
		{
			const string adminEmail = "admin@smarttrackingtransport.com";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				adminUser = new AppUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					DisplayName = "Admin User",
					EmailConfirmed = true
				};

				await userManager.CreateAsync(adminUser, "Admin123!@#"); // You should change this password in production
				await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
			}
		}
	}
}
