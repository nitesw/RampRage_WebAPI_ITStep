using Core.Interfaces;
using Data.Constants;
using Data.Data;
using Data.Data.Seeders;
using Data.Entities;
using Data.Entities.Identity;
using Data.Models.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace RampRage_WebAPI_ITStep.Extensions
{
    public static class SeedExtensions
    {
        public static async Task SeedAsync(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RampRageDbContext>();

            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                await roleManager.CreateAsync(new () { Name = Roles.Admin });
                await roleManager.CreateAsync(new () { Name = Roles.User });
            }

            if (!context.Users.Any())
            {
                var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = File.ReadAllText(jsonFile, Encoding.UTF8);
                    try
                    {
                        var users = JsonConvert.DeserializeObject<List<SeederUserModel>>(jsonData)
                            ?? throw new JsonException();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                        foreach (var user in users)
                        {
                            var newUser = new UserEntity
                            {
                                UserName = user.UserName,
                                Email = user.Email,
                                ImageUrl = await imageService.SaveImageFromUrlAsync(user.ImageUrl)
                            };
                            var result = await userManager.CreateAsync(newUser, user.Password);
                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(newUser, user.Role);
                            }
                            else Console.WriteLine($"--Error create user {user.Email}--");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--Error parse json--{ex.Message}");
                    }
                }
                else Console.WriteLine($"--Error open file {jsonFile}--");
            }

            if (!context.Categories.Any())
            {
                var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = File.ReadAllText(jsonFile, Encoding.UTF8);
                    try
                    {
                        var categories = JsonConvert.DeserializeObject<List<CategorySeederModel>>(jsonData)
                            ?? throw new JsonException();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                        foreach (var category in categories)
                        {
                            var newCategory = new CategoryEntity
                            {
                                Name = category.Name,
                                Description = category.Description,
                                UserId = category.UserId,
                                ImageUrl = await imageService.SaveImageFromUrlAsync(category.ImageUrl!)
                            };
                            context.Categories.Add(newCategory);
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--Error parse json--{ex.Message}");
                    }
                }
                else Console.WriteLine($"--Error open file {jsonFile}--");
            }
        }
    }
}
