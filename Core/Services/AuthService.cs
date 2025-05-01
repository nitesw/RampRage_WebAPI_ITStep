using AutoMapper;
using Core.Dtos.User;
using Core.Interfaces;
using Data.Constants;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Core.Services
{
    public class AuthService(
        UserManager<UserEntity> userManager,
        IMapper mapper,
        IImageService imageService,
        IJwtService jwtService
        ) : IAuthService
    {
        private readonly UserManager<UserEntity> userManager = userManager;
        private readonly IMapper mapper = mapper;
        private readonly IImageService imageService = imageService;
        private readonly IJwtService jwtService = jwtService;

        public async Task<bool> Register(UserRegisterDto dto)
        {
            var isExistsByEmail = await userManager.FindByEmailAsync(dto.Email);
            if (isExistsByEmail != null)
                throw new Exception($"Email {dto.Email} is already in use.");

            var isExistsByUsername = await userManager.FindByNameAsync(dto.UserName);
            if (isExistsByUsername != null)
                throw new Exception($"Username {dto.UserName} is already in use.");

            var entity = mapper.Map<UserEntity>(dto);
            entity.ImageUrl = await imageService.SaveImageFromUrlAsync(
                "https://static.vecteezy.com/system/resources/thumbnails/002/318/271/small_2x/user-profile-icon-free-vector.jpg"
            );

            var result = await userManager.CreateAsync(entity, dto.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                throw new Exception($"Registration failed: {error.Description}");
            }

            await userManager.AddToRoleAsync(entity, Roles.User);
            return true;
        }

        public async Task<string> Login(UserAuthDto dto)
        {
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            bool isEmail = Regex.IsMatch(dto.Identifier.Trim(), pattern);

            var user = new UserEntity();
            if (isEmail)
                user = await userManager.FindByEmailAsync(dto.Identifier);
            else
                user = await userManager.FindByNameAsync(dto.Identifier);

            if (user == null)
                throw new Exception("User data is incorrect!");

            bool isPasswordCorrect = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordCorrect)
                throw new Exception("User data is incorrect!");

            var token = await jwtService.CreateTokenAsync(user);
            return token;
        }
    }
}
