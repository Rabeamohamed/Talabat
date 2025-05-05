using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;


namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Confirgure Services Add Services to the Container
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });


            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });

            #endregion

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            var app = builder.Build();

            #region Update Database

            //StoreContext dbContext = new StoreContext(); // Invalid way to create a DbContext
            //await dbContext.Database.MigrateAsync();

            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = Services.GetRequiredService<StoreContext>();
                await dbContext.Database.MigrateAsync(); // Update the database after Migration
                
                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync(); 
                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

                await StoreContextSeed.SeedAsync(dbContext); // Seed the database with initial data



            }
            catch (Exception ex )
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An error occurred during Applying Migration");
            }

            #endregion

            #region Configure - Configure the HTTP Request Pipeline
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.UseSwaggerMiddleWares();

            }

            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers(); 

            #endregion

            app.Run();
        }
    }
}
