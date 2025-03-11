using Infrastucture.DbContexts;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SmartTrackingTransport.Extensions
{
	public static class IdentityServiceExtension
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services , IConfiguration _config) {
			var builder = services.AddIdentityCore<AppUser>();
			builder = new IdentityBuilder(builder.UserType,builder.Services);
			builder.AddEntityFrameworkStores<AppIdentityDbContext>();
			builder.AddSignInManager<SignInManager<AppUser>>();
			builder.AddDefaultTokenProviders();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),
					ValidateIssuer = true,
					ValidIssuer = _config["Token:Issuer"],
					ValidateAudience = false
				};
			});
			return services;
		}
	}
}
