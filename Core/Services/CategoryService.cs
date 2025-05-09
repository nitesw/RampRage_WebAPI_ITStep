using AutoMapper;
using Core.Dtos.Category;
using Core.Interfaces;
using Data.Data;
using Data.Entities;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Services
{
    public class CategoryService(
        RampRageDbContext context,
        IImageService imageService,
        IMapper mapper,
        UserManager<UserEntity> userManager
        ) : ICategoryService
    {
        public async Task<CategoryDto> Create(CategoryCreateDto dto, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var category = mapper.Map<CategoryEntity>(dto);
            category.ImageUrl = await imageService.SaveImageAsync(dto.Image);
            category.UserId = user.Id;

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            var categoryDto = mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task Delete(int id)
        {
            var category = await context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
                throw new Exception("Category not found");

            if (category.ImageUrl != null)
            {
                imageService.DeleteImageIfExists(category.ImageUrl);
            }

            await Task.Run(() => {
                context.Categories.Remove(category);
            });

            await context.SaveChangesAsync();
        }

        public async Task Edit(CategoryEditDto dto)
        {
            var category = await context.Categories.SingleOrDefaultAsync(x => x.Id == dto.Id);
            if (category == null)
                throw new Exception("Category not found");

            category = mapper.Map(dto, category);
            if (dto.Image != null)
            {
                string deleteImageUrl = category.ImageUrl!;
                category.ImageUrl = await imageService.SaveImageAsync(dto.Image);
                imageService.DeleteImageIfExists(deleteImageUrl);
            }

            await context.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            var list = await context.Categories
                .ToListAsync();

            return mapper.Map<List<CategoryDto>>(list);
        }
    }
}
