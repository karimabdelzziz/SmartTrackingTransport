
using Infrastructure.Interfaces;
using Infrastructure.Repos;
using Infrastucture.DbContexts;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Services.IEmailService;
using Services.Services.TokenService;
using Services.Services.UserService;
using SmartTrackingTransport.Extensions;


namespace SmartTrackingTransport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Add services to the container.
           
            builder.Services.AddControllers();
		
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
			builder.Services.AddDbContext<TransportContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnection"));
			});
			builder.Services.AddOpenApi();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddIdentityService(builder.Configuration);
			var app = builder.Build();

            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                
            }

            app.UseHttpsRedirection();
			app.UseAuthentication();

			app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
