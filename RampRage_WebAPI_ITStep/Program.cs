
using Core;
using Core.Interfaces;
using Core.Services;
using Data;
using Data.Data;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RampRage_WebAPI_ITStep.Extensions;
using System.Text;
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

            var singinKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["JwtSecretKey"]
                        ?? throw new NullReferenceException("JwtSecretKey")
                )
            );
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = singinKey,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddCustomServices();
            builder.Services.AddAutoMapper();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Jwt Auth header using the Bearer scheme",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    }
                );
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                     {
                         new OpenApiSecurityScheme {
                             Reference = new OpenApiReference {
                                 Id = "Bearer",
                                 Type = ReferenceType.SecurityScheme
                             }
                         },
                         new List<string>()
                     }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            var imagesFolder = builder.Configuration.GetValue<string>("ImagesDir") ?? "";
            var saveDir = Path.Combine(builder.Environment.ContentRootPath, imagesFolder);
            Directory.CreateDirectory(saveDir);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(saveDir),
                RequestPath = $"/{imagesFolder}"
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RampRage API v1"));

            app.UseHttpsRedirection();

            app.UseCors("front-end-cors-policy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.SeedAsync();

            app.Run();
        }
    }
}
