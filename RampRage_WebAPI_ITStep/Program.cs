
using Core;
using Core.Interfaces;
using Core.Services;
using Data;
using Data.Data;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RampRage_WebAPI_ITStep.Extensions;
using System.Threading.Tasks;

namespace RampRage_WebAPI_ITStep
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            string connectionString = builder.Configuration.GetConnectionString("NeonDB")!;
            builder.Services.AddRampRageDbContext(connectionString);

            builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<RampRageDbContext>().AddDefaultTokenProviders();

            builder.Services.AddCustomServices();
            builder.Services.AddAutoMapper();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RampRage API v1"));

            app.UseHttpsRedirection();

            app.UseCors("front-end-cors-policy");

            app.UseAuthorization();

            app.MapControllers();

            await app.SeedAsync();

            app.Run();
        }
    }
}
