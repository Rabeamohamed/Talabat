using Microsoft.EntityFrameworkCore;
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

            #endregion

            var app = builder.Build();

            #region Update Database

            //StoreContext dbContext = new StoreContext(); // Invalid way to create a DbContext
            //await dbContext.Database.MigrateAsync();

            using var Scope = app.Services.CreateScope();
            var services = Scope.ServiceProvider;
            var DbContext = services.GetRequiredService<StoreContext>();
            await DbContext.Database.MigrateAsync(); // Update the database after Migration

            #endregion

            #region Configure - Configure the HTTP Request Pipeline
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
