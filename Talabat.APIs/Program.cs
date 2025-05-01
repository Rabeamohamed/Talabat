using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;


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

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            builder.Services.AddAutoMapper(typeof(MappingProfiles)); // Add AutoMapper to the services

            builder.Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                 {
                     var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                         .SelectMany(P => P.Value.Errors)
                         .Select(E => E.ErrorMessage).ToArray();

                     var ValidationErrorResponse = new ApiValidationErrorResponse()
                     {
                         Errors = errors
                     };
                     return new BadRequestObjectResult(ValidationErrorResponse);
                 };
            });
            #endregion

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithRedirects("errors/{0}");

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
