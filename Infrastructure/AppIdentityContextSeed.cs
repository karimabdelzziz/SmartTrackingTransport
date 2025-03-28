﻿using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public class AppIdentityContextSeed
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if(!userManager.Users.Any())
			{
				var user = new AppUser
				{
					DisplayName = "Ahmed",
					Email = "Ahmed@gmail.com",
					UserName = "ahmedElnagar",
					Address = new Address
					{
						FirstName = "Ahmed",
						LastName = "Elnagar",
					}
				};
				await userManager.CreateAsync(user,"Password123!");
			}
		}
	}
}
